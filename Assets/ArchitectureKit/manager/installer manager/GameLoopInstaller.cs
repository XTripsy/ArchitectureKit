namespace MyGameLoop
{
    [System.Serializable]
    internal struct GameLoopGroup
    {

    }

    internal sealed class GameLoopInstaller : IGameLoopInstaller
    {
        public void Install(IBootstrapContext installer)
        {
            IUpdateHandler updateHandler = new UpdateHandler();
            IGameLoopManager temp = new GameLoopManager(updateHandler as UpdateHandler);
            installer.IRegister(temp);
            installer.ISetGameLoop(temp);
        }
    }
}
