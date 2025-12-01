using Namespace_ExitLobby_Event;
using Namespace_PlayerController_Event;
using UnityEngine;

namespace Namespace_ExitLobby_Event
{
    internal readonly struct ExitLobbyPlayer : IEvent { }
}

namespace Namespace_Trigger
{
    internal sealed class TriggerExitLobby : ITriggerSystem
    {
        private string _nameEvent;
        private readonly IEventBus _bus;
        private bool _bIsActive;

        public TriggerExitLobby(IEventBus bus)
        {
            _nameEvent = "test";
            _bus = bus;

            _bus.ISubscribe<TriggerEnter>(TriggerEnter);
            _bus.ISubscribe<TriggerExit>(TriggerExit);
            _bus.ISubscribe<ActionPlayerInteract>(_ => Interact());
        }

        private void TriggerEnter(TriggerEnter enter)
        {
            if (enter.nameEvent != _nameEvent) return;
            _bIsActive = true;
        }

        private void TriggerExit(TriggerExit exit)
        {
            if (exit.nameEvent != _nameEvent) return;
            _bIsActive = false;
        }

        private void Interact()
        {
            if (!_bIsActive) return;

            _bus.IPublish(new ExitLobbyPlayer());
            Debug.LogWarning("INTERACT KOTOL");
        }
    }
}
