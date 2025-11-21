public sealed class GameStateInstaller : IGameStateInstaller
{
    public void Install(IBootstrapContext installer)
    {
        IEventBus bus = installer.IGetBus;
        IStateRegistry registry = installer.IGetStateRegistry;
        GameState gameState = installer.IGetGameState;
        StateController _stateController = new StateController(bus, gameState, registry);
    }
}