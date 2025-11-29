using Namespace_ActionSpawnPlayer_Event;
using UnityEngine;

namespace Namespace_InputLobby_Event
{
    internal readonly struct ActionJoinLobbyState : IEvent { }
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
