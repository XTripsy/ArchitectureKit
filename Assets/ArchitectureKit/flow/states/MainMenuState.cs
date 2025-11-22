using Namespace_StateMainMenu_Event;

namespace Namespace_StateMainMenu_Event
{
    internal readonly struct MainMenuStateEnter : IEvent { }
    internal readonly struct MainMenuStateExit : IEvent { }

}

namespace Namespace_StateMainMenu
{

    public sealed class MainMenuState : IState
    {
        private readonly IEventBus _bus;

        public MainMenuState(IEventBus temp_bus)
        {
            _bus = temp_bus;
        }

        public void IOnEnter()
        {
            _bus.IPublish(new MainMenuStateEnter());
        }

        public void IOnExit()
        {
            _bus.IPublish(new MainMenuStateExit());
        }
    }
}
