using Namespace_Level;

namespace Namespace_InputMainMenu_Event
{
    internal readonly struct ActionPlayMainMenuState : IEvent { }
    internal readonly struct ActionBrowseMainMenuState : IEvent { }
    internal readonly struct ActionJoinMainMenuState : IEvent { }
}

namespace Namespace_InputMainMenu
{
    internal sealed class InputActionMainMenuState
    {
        IEventBus _bus;

        public InputActionMainMenuState(IEventBus bus)
        {
            _bus = bus;
        }

        public void CreateRoomMainMenu()
        {
            _bus.IPublish(new LevelRequest("gameplay_scene"));
            _bus.IPublish(new RequestStateEnter("lobby_state"));
        }

        public void BrowseMainMenu()
        {
            _bus.IPublish(new RequestStateEnter("browse_state"));
        }

        public void JoinMainMenu()
        {
            _bus.IPublish(new LevelRequest("gameplay_scene"));
            _bus.IPublish(new RequestStateEnter("lobby_state"));
        }
    }
}
