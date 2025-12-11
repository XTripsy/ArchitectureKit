using Namespace_Level;
using PurrLobby;

namespace Namespace_InputMainMenu_Event
{
    internal readonly struct ActionPlayMainMenuState : IEvent { }
}

namespace Namespace_InputMainMenu
{
    internal sealed class InputActionMainMenuState
    {
        private IEventBus _bus;
        private LobbyManager _lobbyManager;

        public InputActionMainMenuState(IEventBus bus, LobbyManager lm)
        {
            _bus = bus;
            _lobbyManager = lm;
        }

        public void CreateRoomMainMenu()
        {
            _lobbyManager.CreateRoom();
        }
    }
}
