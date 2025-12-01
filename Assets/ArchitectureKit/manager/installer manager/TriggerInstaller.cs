namespace Namespace_Trigger
{
    internal sealed class TriggerInstaller : ITriggerInstaller
    {
        public void Install(IBootstrapContext installer)
        {
            IEventBus bus = installer.IGetBus;
            ITriggerManager temp = new TriggerManager(bus);
            installer.IRegister(temp);
            _InstallTriggerSystem(bus, temp);
        }

        private void _InstallTriggerSystem(IEventBus bus, ITriggerManager temp)
        {
            ITriggerSystem temp_system = new TriggerExitLobby(bus);
            temp.IAddTriggerSystem("trigger_exit_lobby", temp_system);
        }
    }
}
