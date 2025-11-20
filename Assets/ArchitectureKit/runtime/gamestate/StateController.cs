using UnityEngine;

public sealed class StateController
{
    private readonly IEventBus _bus;
    private readonly GameState _state;
    private readonly IKeyFactory<FactoryState.EState, IState> _factory;

    public StateController(IEventBus bus, GameState state, IKeyFactory<FactoryState.EState, IState> factory)
    {
        _bus = bus;
        _state = state;
        _factory = factory;

        _bus.ISubscribe<RequestMainMenuStateEnter>(_ => _state.Change(_factory.Create(FactoryState.EState.eMainMenuState)));
        _bus.ISubscribe<RequestGameplayStateEnter>(_ => _state.Change(_factory.Create(FactoryState.EState.eGameplayState)));
    }
}
