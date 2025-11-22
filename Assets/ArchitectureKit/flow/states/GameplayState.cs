using Namespace_StateGameplay_Event;

namespace Namespace_StateGameplay_Event
{
    internal readonly struct GameplayStateEnter : IEvent { }
    internal readonly struct GameplayStateExit : IEvent { }
}

namespace Namespace_StateGameplay
{
    public sealed class GameplayState : IState
    {
        private readonly IEventBus _bus;

        public GameplayState(IEventBus temp_bus)
        {
            _bus = temp_bus;
        }

        public void IOnEnter()
        {
            _bus.IPublish(new GameplayStateEnter());
        }

        public void IOnExit()
        {
            _bus.IPublish(new GameplayStateExit());
        }
    }
}
