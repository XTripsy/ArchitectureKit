using UnityEngine;

namespace Namespace_InputGameplay_Event
{
    internal readonly struct ActionClickGameplayState : IEvent { }
    internal readonly struct ActionPauseGameplayState : IEvent { }
}

namespace Namespace_InputGameplay
{
    internal sealed class InputActionGameplayState
    {
        IEventBus _bus;

        public InputActionGameplayState(IEventBus bus)
        {
            _bus = bus;
        }

        public void ClickGameplay()
        {
            Debug.LogError("CLICK");
        }

        public void PauseGameplay()
        {
            _bus.IPublish(new RequestStateEnter("pause_state"));
        }
    }
}