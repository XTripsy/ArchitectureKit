using System;
using System.Collections.Generic;

public sealed class BootstrapContext : IBootstrapContext
{
    private readonly IEventBus _bus;
    private IGameLoopManager _gameLoop;
    private readonly IStateRegistry _stateRegistry;
    private readonly GameState _gameState;
    private readonly Dictionary<Type, object> _services = new();
    private readonly Dictionary<Type, object> _groups = new();

    public BootstrapContext(IEventBus bus, IStateRegistry stateRegistry, GameState gameState, params object[] groups)
    {
        _bus = bus;
        _stateRegistry = stateRegistry;
        _gameState = gameState;

        foreach (var g in groups)
            _groups[g.GetType()] = g;
    }

    public IEventBus IGetBus 
        => _bus;

    public void ISetGameLoop(IGameLoopManager gameLoopManager)
    {
        _gameLoop = gameLoopManager;
    }

    public IGameLoopManager IGetGameLoop 
        => _gameLoop;

    public IStateRegistry IGetStateRegistry
        => _stateRegistry;

    public GameState IGetGameState 
        => _gameState;

    public T IGetGroup<T>() where T : struct 
        => _groups.TryGetValue(typeof(T), out var g) ? (T)g : default;

    public void IRegister<TService>(TService instance)
        => _services[typeof(TService)] = instance;

    public TService IResolve<TService>()
        => (TService)_services[typeof(TService)];
}