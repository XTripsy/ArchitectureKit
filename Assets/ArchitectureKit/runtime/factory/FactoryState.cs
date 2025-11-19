using System;
using System.Collections.Generic;

public sealed class FactoryState : IKeyFactory<FactoryState.EState, IState>
{
    public enum EState
    {
        eMainMenuState,
        eGameplayState
    }

    private readonly Dictionary<EState, Func<IState>> _map = new();

    public FactoryState(IEventBus _bus)
    {
        _map[EState.eMainMenuState] = () => new MainMenuState(_bus);
        _map[EState.eGameplayState] = () => new GameplayState(_bus);
    }

    public IState Create(EState key) => _map[key]();
}