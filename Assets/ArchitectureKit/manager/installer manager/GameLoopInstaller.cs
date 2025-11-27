namespace Namespace_GameLoop
{
    [System.Serializable]
    internal struct GameLoopGroup
    {

    }

    internal sealed class GameLoopInstaller : IGameLoopInstaller
    {
        public void Install(IBootstrapContext installer)
        {
            IGameLoopManager temp = new GameLoopManager();
            installer.IRegister(temp);
            installer.IGetGameLoop = temp;
        }
    }
}
