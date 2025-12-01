using UnityEngine;
using PurrNet;
using PurrLobby;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

public class LocalMockLobbyProvider : MonoBehaviour, ILobbyProvider
{
    public event UnityAction<string> OnLobbyJoinFailed;
    public event UnityAction OnLobbyLeft;
    public event UnityAction<Lobby> OnLobbyUpdated;
    public event UnityAction<List<LobbyUser>> OnLobbyPlayerListUpdated;
    public event UnityAction<List<FriendUser>> OnFriendListPulled;
    public event UnityAction<string> OnError;

    private string _localUserId;
    private string _currentLobbyId;
    private Dictionary<string, string> _localLobbyData = new Dictionary<string, string>();

    private string _sharedFilePath;
    private const float POLL_INTERVAL = 0.3f;
    private const float BROWSE_POLL_INTERVAL = 1.0f;
    private bool _isPolling;
    private bool _isBrowsePolling;
    private int _lastMemberCount = 0;
    private int _lastReadyCount = 0;
    private int _lastBrowseCount = 0;
    private Dictionary<string, string> _currentBrowseFilters;

    private string SharedFilePath
    {
        get
        {
            if (string.IsNullOrEmpty(_sharedFilePath))
            {
                _sharedFilePath = Path.Combine(Application.persistentDataPath, "mock_lobbies.json");
            }
            return _sharedFilePath;
        }
    }

    [System.Serializable]
    private class SharedLobbyState
    {
        public List<LobbyData> lobbies = new List<LobbyData>();
    }

    [System.Serializable]
    private class LobbyData
    {
        public string lobbyId;
        public string name;
        public int maxPlayers;
        public bool started;
        public List<string> memberIds = new List<string>();
        public List<string> memberNames = new List<string>();
        public List<bool> memberReady = new List<bool>();
        public Dictionary<string, string> properties = new Dictionary<string, string>();
        public long lastUpdateTicks;
    }

    public Task InitializeAsync()
    {
        _localUserId = System.Guid.NewGuid().ToString();
        Debug.Log($"[MockLobby] Initialized with UserId: {_localUserId}");
        Debug.Log($"[MockLobby] Shared file path: {SharedFilePath}");

        if (!File.Exists(SharedFilePath))
        {
            SaveSharedState(new SharedLobbyState());
        }

        return Task.CompletedTask;
    }

    public Task<string> GetLocalUserIdAsync()
    {
        return Task.FromResult(_localUserId);
    }

    public async Task<Lobby> CreateLobbyAsync(int maxPlayers, Dictionary<string, string> lobbyProperties = null)
    {
        var state = LoadSharedState();

        var lobbyId = System.Guid.NewGuid().ToString().Substring(0, 8);
        var lobbyName = $"Lobby_{lobbyId}";

        if (lobbyProperties == null)
            lobbyProperties = new Dictionary<string, string>();

        lobbyProperties["ServerAddress"] = "127.0.0.1:7777";

        var lobbyData = new LobbyData
        {
            lobbyId = lobbyId,
            name = lobbyName,
            maxPlayers = maxPlayers,
            started = false,
            memberIds = new List<string> { _localUserId },
            memberNames = new List<string> { $"Player_{_localUserId.Substring(0, 4)}" },
            memberReady = new List<bool> { false },
            properties = lobbyProperties,
            lastUpdateTicks = System.DateTime.UtcNow.Ticks
        };

        state.lobbies.Add(lobbyData);
        SaveSharedState(state);

        _currentLobbyId = lobbyId;
        _lastMemberCount = 1;
        _lastReadyCount = 0;
        StartPolling();

        var lobby = ConvertToLobby(lobbyData, true);
        Debug.Log($"[MockLobby] Created lobby: {lobbyId} with {lobby.Members.Count} members");

        await Task.Delay(100);
        OnLobbyUpdated?.Invoke(lobby);
        return lobby;
    }

    public async Task<List<Lobby>> SearchLobbiesAsync(int maxRoomsToFind = 10, Dictionary<string, string> filters = null)
    {
        _currentBrowseFilters = filters;

        var state = LoadSharedState();
        var results = new List<Lobby>();

        foreach (var lobbyData in state.lobbies.Where(l => !l.started).Take(maxRoomsToFind))
        {
            if (filters != null)
            {
                bool matches = true;
                foreach (var filter in filters)
                {
                    if (!lobbyData.properties.TryGetValue(filter.Key, out var value) || value != filter.Value)
                    {
                        matches = false;
                        break;
                    }
                }
                if (!matches) continue;
            }

            results.Add(ConvertToLobby(lobbyData, false));
        }

        _lastBrowseCount = results.Count;
        Debug.Log($"[MockLobby] Found {results.Count} lobbies");

        // Start browse polling to keep list updated
        StartBrowsePolling();

        await Task.Delay(50);
        return results;
    }

    public async Task<Lobby> JoinLobbyAsync(string lobbyId)
    {
        // Stop browse polling when joining a lobby
        StopBrowsePolling();

        var state = LoadSharedState();
        var lobbyData = state.lobbies.FirstOrDefault(l => l.lobbyId == lobbyId);

        if (lobbyData == null)
        {
            OnLobbyJoinFailed?.Invoke($"Lobby {lobbyId} not found");
            return new Lobby { IsValid = false };
        }

        if (lobbyData.memberIds.Count >= lobbyData.maxPlayers)
        {
            OnLobbyJoinFailed?.Invoke("Lobby is full");
            return new Lobby { IsValid = false };
        }

        if (!lobbyData.memberIds.Contains(_localUserId))
        {
            lobbyData.memberIds.Add(_localUserId);
            lobbyData.memberNames.Add($"Player_{_localUserId.Substring(0, 4)}");
            lobbyData.memberReady.Add(false);
            lobbyData.lastUpdateTicks = System.DateTime.UtcNow.Ticks;
            SaveSharedState(state);
        }

        _currentLobbyId = lobbyId;
        _lastMemberCount = lobbyData.memberIds.Count;
        _lastReadyCount = lobbyData.memberReady.Count(r => r);
        StartPolling();

        var lobby = ConvertToLobby(lobbyData, false);
        Debug.Log($"[MockLobby] Joined lobby: {lobbyId} with {lobby.Members.Count} members");

        await Task.Delay(100);
        OnLobbyUpdated?.Invoke(lobby);
        return lobby;
    }

    public Task LeaveLobbyAsync()
    {
        if (string.IsNullOrEmpty(_currentLobbyId)) return Task.CompletedTask;

        var state = LoadSharedState();
        var lobbyData = state.lobbies.FirstOrDefault(l => l.lobbyId == _currentLobbyId);

        if (lobbyData != null)
        {
            int index = lobbyData.memberIds.IndexOf(_localUserId);
            if (index >= 0)
            {
                lobbyData.memberIds.RemoveAt(index);
                lobbyData.memberNames.RemoveAt(index);
                lobbyData.memberReady.RemoveAt(index);
                lobbyData.lastUpdateTicks = System.DateTime.UtcNow.Ticks;

                if (lobbyData.memberIds.Count == 0)
                {
                    state.lobbies.Remove(lobbyData);
                }

                SaveSharedState(state);
            }
        }

        StopPolling();
        _currentLobbyId = null;
        _localLobbyData.Clear();
        _lastMemberCount = 0;
        _lastReadyCount = 0;

        Debug.Log("[MockLobby] Left lobby");
        OnLobbyLeft?.Invoke();
        return Task.CompletedTask;
    }

    public Task LeaveLobbyAsync(string lobbyId)
    {
        if (_currentLobbyId == lobbyId)
        {
            return LeaveLobbyAsync();
        }
        return Task.CompletedTask;
    }

    public Task SetIsReadyAsync(string userId, bool isReady)
    {
        if (userId != _localUserId || string.IsNullOrEmpty(_currentLobbyId))
            return Task.CompletedTask;

        var state = LoadSharedState();
        var lobbyData = state.lobbies.FirstOrDefault(l => l.lobbyId == _currentLobbyId);

        if (lobbyData != null)
        {
            int index = lobbyData.memberIds.IndexOf(_localUserId);
            if (index >= 0)
            {
                lobbyData.memberReady[index] = isReady;
                lobbyData.lastUpdateTicks = System.DateTime.UtcNow.Ticks;
                SaveSharedState(state);
                Debug.Log($"[MockLobby] Set ready: {isReady}, triggering update");

                // Force immediate update
                PollLobbyUpdates();
            }
        }

        return Task.CompletedTask;
    }

    public Task SetLobbyDataAsync(string key, string value)
    {
        _localLobbyData[key] = value;

        if (!string.IsNullOrEmpty(_currentLobbyId))
        {
            var state = LoadSharedState();
            var lobbyData = state.lobbies.FirstOrDefault(l => l.lobbyId == _currentLobbyId);

            if (lobbyData != null)
            {
                lobbyData.properties[key] = value;
                lobbyData.lastUpdateTicks = System.DateTime.UtcNow.Ticks;
                SaveSharedState(state);
            }
        }

        return Task.CompletedTask;
    }

    public Task<string> GetLobbyDataAsync(string key)
    {
        _localLobbyData.TryGetValue(key, out var value);
        return Task.FromResult(value ?? string.Empty);
    }

    public Task SetLobbyStartedAsync()
    {
        if (string.IsNullOrEmpty(_currentLobbyId)) return Task.CompletedTask;

        var state = LoadSharedState();
        var lobbyData = state.lobbies.FirstOrDefault(l => l.lobbyId == _currentLobbyId);

        if (lobbyData != null)
        {
            lobbyData.started = true;
            lobbyData.lastUpdateTicks = System.DateTime.UtcNow.Ticks;
            SaveSharedState(state);
            Debug.Log("[MockLobby] Lobby started");
        }

        return Task.CompletedTask;
    }

    public Task<List<LobbyUser>> GetLobbyMembersAsync()
    {
        if (string.IsNullOrEmpty(_currentLobbyId))
            return Task.FromResult(new List<LobbyUser>());

        var state = LoadSharedState();
        var lobbyData = state.lobbies.FirstOrDefault(l => l.lobbyId == _currentLobbyId);

        if (lobbyData == null)
            return Task.FromResult(new List<LobbyUser>());

        var users = new List<LobbyUser>();
        for (int i = 0; i < lobbyData.memberIds.Count; i++)
        {
            users.Add(new LobbyUser
            {
                Id = lobbyData.memberIds[i],
                DisplayName = lobbyData.memberNames[i],
                IsReady = lobbyData.memberReady[i],
                Avatar = null
            });
        }

        return Task.FromResult(users);
    }

    public Task<List<FriendUser>> GetFriendsAsync(LobbyManager.FriendFilter filter)
    {
        return Task.FromResult(new List<FriendUser>());
    }

    public Task InviteFriendAsync(FriendUser user)
    {
        return Task.CompletedTask;
    }

    public Task SetAllReadyAsync()
    {
        return Task.CompletedTask;
    }

    public void Shutdown()
    {
        StopPolling();
        StopBrowsePolling();
    }

    private void StartPolling()
    {
        if (_isPolling) return;
        _isPolling = true;
        InvokeRepeating(nameof(PollLobbyUpdates), POLL_INTERVAL, POLL_INTERVAL);
        Debug.Log("[MockLobby] Started polling for updates");
    }

    private void StopPolling()
    {
        if (!_isPolling) return;
        _isPolling = false;
        CancelInvoke(nameof(PollLobbyUpdates));
        Debug.Log("[MockLobby] Stopped polling");
    }

    private void StartBrowsePolling()
    {
        if (_isBrowsePolling) return;
        _isBrowsePolling = true;
        InvokeRepeating(nameof(PollBrowseUpdates), BROWSE_POLL_INTERVAL, BROWSE_POLL_INTERVAL);
        Debug.Log("[MockLobby] Started browse polling");
    }

    private void StopBrowsePolling()
    {
        if (!_isBrowsePolling) return;
        _isBrowsePolling = false;
        CancelInvoke(nameof(PollBrowseUpdates));
        Debug.Log("[MockLobby] Stopped browse polling");
    }

    private void PollBrowseUpdates()
    {
        var state = LoadSharedState();
        var results = new List<Lobby>();

        foreach (var lobbyData in state.lobbies.Where(l => !l.started))
        {
            if (_currentBrowseFilters != null)
            {
                bool matches = true;
                foreach (var filter in _currentBrowseFilters)
                {
                    if (!lobbyData.properties.TryGetValue(filter.Key, out var value) || value != filter.Value)
                    {
                        matches = false;
                        break;
                    }
                }
                if (!matches) continue;
            }

            results.Add(ConvertToLobby(lobbyData, false));
        }

        if (results.Count != _lastBrowseCount)
        {
            _lastBrowseCount = results.Count;
            Debug.Log($"[MockLobby] Browse list changed: {results.Count} lobbies");
            // Note: PurrLobby doesn't have a browse update event, so UI needs to call SearchLobbiesAsync periodically
        }
    }

    private void PollLobbyUpdates()
    {
        if (string.IsNullOrEmpty(_currentLobbyId)) return;

        var state = LoadSharedState();
        var lobbyData = state.lobbies.FirstOrDefault(l => l.lobbyId == _currentLobbyId);

        if (lobbyData == null)
        {
            Debug.Log("[MockLobby] Lobby deleted, leaving");
            StopPolling();
            _currentLobbyId = null;
            OnLobbyLeft?.Invoke();
            return;
        }

        // Check for changes
        int currentMemberCount = lobbyData.memberIds.Count;
        int currentReadyCount = lobbyData.memberReady.Count(r => r);

        bool hasChanges = currentMemberCount != _lastMemberCount || currentReadyCount != _lastReadyCount;

        if (hasChanges)
        {
            Debug.Log($"[MockLobby] Detected changes - Members: {_lastMemberCount}->{currentMemberCount}, Ready: {_lastReadyCount}->{currentReadyCount}");
            _lastMemberCount = currentMemberCount;
            _lastReadyCount = currentReadyCount;

            var isOwner = lobbyData.memberIds.FirstOrDefault() == _localUserId;
            var lobby = ConvertToLobby(lobbyData, isOwner);

            Debug.Log($"[MockLobby] Invoking OnLobbyUpdated with {lobby.Members.Count} members");
            OnLobbyUpdated?.Invoke(lobby);

            var users = lobby.Members;
            Debug.Log($"[MockLobby] Invoking OnLobbyPlayerListUpdated with {users.Count} members");
            OnLobbyPlayerListUpdated?.Invoke(users);
        }
    }

    private SharedLobbyState LoadSharedState()
    {
        try
        {
            if (!File.Exists(SharedFilePath))
                return new SharedLobbyState();

            var json = File.ReadAllText(SharedFilePath);
            var state = JsonUtility.FromJson<SharedLobbyState>(json);

            if (state == null || state.lobbies == null)
            {
                return new SharedLobbyState();
            }

            return state;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"[MockLobby] Failed to load state: {e.Message}");
            return new SharedLobbyState();
        }
    }

    private void SaveSharedState(SharedLobbyState state)
    {
        try
        {
            var json = JsonUtility.ToJson(state, true);

            // Write to temp file first, then move (atomic operation)
            var tempPath = SharedFilePath + ".tmp";
            File.WriteAllText(tempPath, json);

            if (File.Exists(SharedFilePath))
                File.Delete(SharedFilePath);

            File.Move(tempPath, SharedFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[MockLobby] Failed to save state: {e.Message}");
        }
    }

    private Lobby ConvertToLobby(LobbyData data, bool isOwner)
    {
        var users = new List<LobbyUser>();
        for (int i = 0; i < data.memberIds.Count; i++)
        {
            users.Add(new LobbyUser
            {
                Id = data.memberIds[i],
                DisplayName = data.memberNames[i],
                IsReady = i < data.memberReady.Count ? data.memberReady[i] : false,
                Avatar = null
            });
        }

        return new Lobby
        {
            Name = data.name,
            LobbyId = data.lobbyId,
            MaxPlayers = data.maxPlayers,
            IsValid = true,
            IsOwner = isOwner,
            Members = users,
            Properties = new Dictionary<string, string>(data.properties)
        };
    }

    private void OnDestroy()
    {
        Shutdown();
    }
}