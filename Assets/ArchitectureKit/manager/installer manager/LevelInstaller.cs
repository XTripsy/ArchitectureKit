namespace Namespace_Level
{
    [System.Serializable]
    internal struct LevelGroup
    {

    }

    internal class LevelInstaller : ILevelInstaller
    {
        public void Install(IBootstrapContext installer)
        {
            IEventBus bus = installer.IGetBus;
            ILevelManager temp = new LevelManager(bus);
            installer.IRegister(temp);
        }
    }
}
