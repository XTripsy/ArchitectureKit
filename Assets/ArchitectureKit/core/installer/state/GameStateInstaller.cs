public sealed class GameStateInstaller : IGameStateInstaller
{
    public void Install(IBootstrapContext installer)
    {
        IEventBus bus = installer.IGetBus;
        GameState gameState = installer.IGetGameState;
        IKeyFactory<FactoryState.EState, IState> factory_state = new FactoryState(bus);
        StateController _stateController = new StateController(bus, gameState, factory_state);
    }
}