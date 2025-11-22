using UnityEngine;

namespace Namespace_InputGameplayEvent
{
    internal readonly struct ActionClickGameplayState : IEvent { }
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
    }
}