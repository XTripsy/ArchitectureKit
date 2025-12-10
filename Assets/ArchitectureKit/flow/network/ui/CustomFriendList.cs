using System.Collections.Generic;
using System.Linq;
using PurrLobby;
using PurrNet;
using UnityEngine;

public class CustomFriendList : MonoBehaviour
{
    [SerializeField] private FriendEntry friendEntry;
    [SerializeField] private Transform content;
    [SerializeField] private LobbyManager.FriendFilter filter = LobbyManager.FriendFilter.Online;

    private float _lastUpdateTime;
    private Dictionary<string, FriendEntry> _currentFriends = new Dictionary<string, FriendEntry>();

    [SerializeField] private LobbyManager _lobbyManager;
    private IEventBus _bus;

    public void Init(IEventBus bus, LobbyManager lm)
    {
        Debug.Log("<color=green>FRIENDS LIST INITIALIZED");
        _bus = bus;
        _lobbyManager = lm;
    }

    public void Populate(List<FriendUser> friends)
    {
        var newFriendIds = new HashSet<string>(friends.Select(f => f.Id));
        var existingFriendIds = new HashSet<string>(_currentFriends.Keys);

        foreach (var id in existingFriendIds.Except(newFriendIds))
        {
            Destroy(_currentFriends[id].gameObject);
            _currentFriends.Remove(id);
        }

        foreach (var friend in friends)
        {
            if (!_currentFriends.TryGetValue(friend.Id, out var existingEntry))
            {
                var newEntry = Instantiate(friendEntry, content);
                newEntry.Init(friend, _lobbyManager);
                _currentFriends[friend.Id] = newEntry;
            }
        }
    }

    private void Update()
    {
        if (_lastUpdateTime + 3f < Time.time)
        {
            Debug.Log("<color=yellow> FRIEND LIST UPDATED");
            _lastUpdateTime = Time.time;
            _lobbyManager?.PullFriends(filter);
        }
    }
}