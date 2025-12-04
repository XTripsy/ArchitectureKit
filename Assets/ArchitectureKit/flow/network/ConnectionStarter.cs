using System.Collections;
using PurrNet;
using PurrNet.Logging;
using PurrNet.Transports;
using UnityEngine;
using PurrNet.Steam;
using Unity.Multiplayer.Playmode;
using Steamworks;
using PurrLobby;

#if UTP_LOBBYRELAY
using PurrNet.UTP;
using Unity.Services.Relay.Models;
#endif

public class ConnectionStarter : MonoBehaviour
{
    private NetworkManager _networkManager;
    private LobbyDataHolder _lobbyDataHolder;
    private SteamTransport _steamTransport;

    private void Awake()
    {
        if (!TryGetComponent(out _networkManager))
        {
            PurrLogger.LogError($"Failed to get {nameof(NetworkManager)} component.", this);
        }
        if (!TryGetComponent<SteamTransport>(out _steamTransport))
            PurrLogger.LogError($"Failed to get {nameof(SteamTransport)} component", this);

        _lobbyDataHolder = FindFirstObjectByType<LobbyDataHolder>();
        if (!_lobbyDataHolder)
            PurrLogger.LogError($"Failed to get {nameof(LobbyDataHolder)} component.", this);
    }

    private void Start()
    {
        _networkManager.transport = _steamTransport;
        if (!_networkManager)
        {
            PurrLogger.LogError($"Failed to start connection. {nameof(NetworkManager)} is null!", this);
            return;
        }

        if (!_lobbyDataHolder)
        {
            PurrLogger.LogError($"Failed to start connection. {nameof(LobbyDataHolder)} is null!", this);
            return;
        }

        if (!_steamTransport)
        {
            PurrLogger.LogError($"Failed to start connection. {nameof(SteamTransport)} is null!", this);
            return;
        }

        if (!_lobbyDataHolder.CurrentLobby.IsValid)
        {
            PurrLogger.LogError($"Failed to start connection. Lobby is invalid!", this);
            return;
        }

        if (_networkManager.transport is PurrTransport)
        {
            (_networkManager.transport as PurrTransport).roomName = _lobbyDataHolder.CurrentLobby.LobbyId;
        }

        if (!ulong.TryParse(_lobbyDataHolder.CurrentLobby.LobbyId, out ulong ulongId))
        {
            Debug.LogError($"Failed to parse lobbyid into ulong", this);
            return;
        }

        var lobbyOwner = SteamMatchmaking.GetLobbyOwner(new CSteamID(ulongId));
        if (!lobbyOwner.IsValid())
        {
            Debug.LogError($"FAILED TO GET LOBBY OWNER FROM PARSED LOBBY ID");
            return;
        }

        if (_lobbyDataHolder.CurrentLobby.IsOwner)
            _networkManager.StartServer();
        StartCoroutine(StartClient());

        // if (CurrentPlayer.IsMainEditor)
        //     _networkManager.StartServer();
        // StartCoroutine(StartClient());
    }

    private IEnumerator StartClient()
    {
        yield return new WaitForSeconds(1f);
        _networkManager.StartClient();
    }
}

