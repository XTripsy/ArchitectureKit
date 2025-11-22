using Namespace_StatePause_Event;

namespace Namespace_StatePause_Event
{
    internal readonly struct PauseStateEnter : IEvent { }
    internal readonly struct PauseStateExit : IEvent { }
}

namespace Namespace_StatePause
{
    public sealed class PauseState : IState
    {
        IEventBus _bus;

        public PauseState(IEventBus bus)
        {
            _bus = bus;
        }

        public void IOnEnter()
        {
            _bus.IPublish(new PauseStateEnter());
        }

        public void IOnExit()
        {
            _bus.IPublish(new PauseStateExit());
        }
    }
}
