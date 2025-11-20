public interface IBootstrapContext
{
    IEventBus IGetBus { get; }
    void ISetGameLoop(IGameLoopManager gameLoopManager);
    IGameLoopManager IGetGameLoop { get; }
    GameState IGetGameState { get; }
    T IGetGroup<T>() where T : struct;
    void IRegister<TService>(TService instance);
    TService IResolve<TService>();
}