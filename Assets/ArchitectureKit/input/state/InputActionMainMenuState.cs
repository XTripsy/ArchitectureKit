using Namespace_Level;

namespace Namespace_InputMainMenuEvent
{
    internal readonly struct ActionPlayMainMenuState : IEvent { }
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

        public void PlayMainMenu()
        {
            _bus.IPublish(new LevelRequest("gameplay_scene"));
            _bus.IPublish(new RequestStateEnter("gameplay_state"));
        }
    }
}
