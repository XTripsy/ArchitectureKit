using Namespace_GameState;

public interface IBootstrapContext
{
    IEventBus IGetBus { get; }
    IGameLoopManager IGetGameLoop { get; set; }
    IStateRegistry IGetStateRegistry {  get; }
    GameState IGetGameState { get; }
    T IGetGroup<T>() where T : struct;
    void IRegister<TService>(TService instance);
    TService IResolve<TService>();
}