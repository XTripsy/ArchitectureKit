using Namespace_ActionSpawnPlayer_Event;
using UnityEngine.InputSystem;

namespace Namespace_InputLobby_Event
{
    public readonly struct ActionJoinLobbyState : IEvent 
    {
        public readonly InputDevice device;

        public ActionJoinLobbyState(InputDevice device)
        {
            this.device = device;
        }
    }
}

namespace Namespace_InputLobby
{
    internal sealed class InputActionLobbyState
    {
        private IEventBus _bus;

        public InputActionLobbyState(IEventBus bus)
        {
            _bus = bus;
        }

        public void JoinLobby()
        {
            _bus.IPublish(new ActionSpawnPlayer());            
        }
    }
}
