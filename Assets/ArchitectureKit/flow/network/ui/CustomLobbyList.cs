using System.Collections.Generic;
using PurrLobby;
using UnityEngine;


public class CustomLobbyList : MonoBehaviour
{
    [SerializeField] private LobbyEntry _lobbyEntryPrefab;
    [SerializeField] private Transform _content;

    private LobbyManager _lobbyManager;
    private IEventBus _bus;
    public void Init(IEventBus b, LobbyManager lm)
    {
        _bus = b;
        _lobbyManager = lm;
    }

    public void Populate(List<Lobby> rooms)
    {
        foreach (Transform child in _content)
            Destroy(child.gameObject);

        foreach (var room in rooms)
        {
            var entry = Instantiate(_lobbyEntryPrefab, _content);
            entry.Init(room, _lobbyManager);
        }
    }
}
