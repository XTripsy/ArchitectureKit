namespace Namespace_Player
{
    internal sealed class PlayerInstaller : IPlayerInstaller
    {
        public void Install(IBootstrapContext installer)
        {
            IEventBus bus = installer.IGetBus;
            IPlayerManager temp = new PlayerManager(installer.IGetGameLoop);
            installer.IRegister(temp);
        }
    }
}
