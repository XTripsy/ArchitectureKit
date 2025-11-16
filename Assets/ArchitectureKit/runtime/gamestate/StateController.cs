using UnityEngine;

public sealed class StateController
{
    private readonly IEventBus _bus;
    private readonly GameState _state;

    public StateController(IEventBus bus, GameState state)
    {
        _bus = bus; 
        _state = state;

        _bus.ISubscribe<RequestMainMenuStateEnter>(_ => _state.Change(new MainMenuState(_bus)));
        _bus.ISubscribe<RequestGameplayStateEnter>(_ => _state.Change(new GameplayState(_bus)));
    }
}
