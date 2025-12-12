using System.Collections;
using PurrNet;
using PurrNet.Logging;
using PurrNet.Transports;
using UnityEngine;
using PurrNet.Steam;
using Steamworks;
using PurrLobby;

public class ConnectionStarter : MonoBehaviour
{
    private NetworkManager _networkManager;
    private LobbyDataHolder _lobbyDataHolder;

    private IEventBus _bus;
    private IUIManager _ui;

    public void Init(IEventBus bus, IUIManager ui)
    {
        _bus = bus;
        _ui = ui;
    }

    private void Awake()
    {
        if (!TryGetComponent(out _networkManager))
        {
            PurrLogger.LogError($"Failed to get {nameof(NetworkManager)} component.", this);
        }

        _lobbyDataHolder = FindFirstObjectByType<LobbyDataHolder>();
        if (!_lobbyDataHolder)
            PurrLogger.LogError($"Failed to get {nameof(LobbyDataHolder)} component.", this);
    }

    private void Start()
    {
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

        if (!_lobbyDataHolder.CurrentLobby.IsValid)
        {
            PurrLogger.LogError($"Failed to start connection. Lobby is invalid!", this);
            return;
        }

        if (_networkManager.transport is PurrTransport)
        {
            (_networkManager.transport as PurrTransport).roomName = _lobbyDataHolder.CurrentLobby.LobbyId;
        }

        if (_lobbyDataHolder.CurrentLobby.IsOwner)
        {
            _networkManager.StartServer();
            Debug.Log("<color=red> you are the HOST");
        }
        StartCoroutine(StartClient());
    }

    private IEnumerator StartClient()
    {
        yield return new WaitForSeconds(1f);
        _networkManager.StartClient();
        _ui.IHide("ui-loading");
        Debug.Log("<color=green>you are the CLIENT");
    }
}

