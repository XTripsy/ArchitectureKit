public sealed class GameStateInstaller : IGameStateInstaller
{
    public void Install(IBootstrapContext installer)
    {
        IEventBus bus = installer.IGetBus;
        IKeyFactory<FactoryState.EState, IState> factory_state = new FactoryState(bus);
        GameState gameState = new GameState();
        StateController _stateController = new StateController(bus, gameState, factory_state);
    }
}