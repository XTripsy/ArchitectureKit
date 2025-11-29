using Namespace_StateLobby_Event;

namespace Namespace_StateLobby_Event
{
    internal readonly struct LobbyStateEnter : IEvent { }
    internal readonly struct LobbyStateExit : IEvent { }
}

namespace Namespace_StateLobby
{
    internal sealed class LobbyState : IState
    {
        private readonly IEventBus _bus;

        public LobbyState(IEventBus temp_bus)
        {
            _bus = temp_bus;
        }

        public void IOnEnter()
        {
            _bus.IPublish(new LobbyStateEnter());
        }

        public void IOnExit()
        {
            _bus.IPublish(new LobbyStateExit());
        }
    }
}
