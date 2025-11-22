namespace Namespace_GameState
{
    internal sealed class StateController
    {
        private readonly IEventBus _bus;
        private readonly GameState _state;
        private readonly IStateRegistry _registry;

        public StateController(IEventBus bus, GameState state, IStateRegistry registry)
        {
            _bus = bus;
            _state = state;
            _registry = registry;

            _bus.ISubscribe<RequestStateEnter>(OnRequestStateEnter);
        }

        private void OnRequestStateEnter(RequestStateEnter e)
        {
            var next = _registry.ICreate(e.id);
            if (next == null) return;
            _state.Change(next);
        }
    }
}
