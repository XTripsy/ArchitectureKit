using Namespace_ActionSpawnPlayer_Event;
using PurrLobby;
using UnityEngine;

namespace Namespace_InputLobby_Event
{
    internal readonly struct ActionReadyLobbyState : IEvent { }
    internal readonly struct ActionLeaveLobbyState : IEvent { }
}

namespace Namespace_InputLobby
{
    internal sealed class InputActionLobbyState
    {
        private IEventBus _bus;
        private LobbyManager _lobbyManager;

        public InputActionLobbyState(IEventBus bus, LobbyManager lm)
        {
            _bus = bus;
            _lobbyManager = lm;
        }

        public void ReadyLobby()
        {
            // _bus.IPublish(new ActionSpawnPlayer());
            _lobbyManager.ToggleLocalReady();
            Debug.Log("<color=green>READY!!!");
        }

        public void LeaveLobby()
        {
            _lobbyManager.LeaveLobby();
        }

    }
}
