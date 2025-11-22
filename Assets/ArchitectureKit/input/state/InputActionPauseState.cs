namespace Namespace_InputPause_Event
{
    internal readonly struct ActionResumePauseState : IEvent { }
}

namespace Namespace_InputPause
{
    internal sealed class InputActionPauseState
    {
        IEventBus _bus;

        public InputActionPauseState(IEventBus bus)
        {
            _bus = bus;
        }

        public void ResumePause()
        {
            _bus.IPublish(new RequestStateEnter("gameplay_state"));
        }
    }
}
