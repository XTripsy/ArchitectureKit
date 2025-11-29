using Namespace_ActionSpawnPlayer_Event;

namespace Namespace_ActionSpawnPlayer_Event
{
    internal readonly struct ActionSpawnPlayer : IEvent { }
    internal readonly struct ActionDeSpawnPlayer : IEvent { }
}

namespace Namespace_ActionSpawnPlayer
{
    internal sealed class ActionPlayer
    {
        IEventBus _bus;
        IGameLoopManager _gameLoopManager;
        IObjectManager _objectManager;

        public ActionPlayer(IEventBus bus, IGameLoopManager gameLoopManager, IObjectManager objectManager)
        {
            _bus = bus;
            _gameLoopManager = gameLoopManager;
            _objectManager = objectManager;

            _bus.ISubscribe<ActionSpawnPlayer>(_ => _SpawnPlayer());
            _bus.ISubscribe<ActionDeSpawnPlayer>(_ => _DeSpawnPlayer());
        }

        private void _SpawnPlayer()
        {
            _objectManager.IActive("player");

            IUpdateManager player_state = new PlayerStateManager();
            _gameLoopManager.IRegister("player_state", player_state);
            _gameLoopManager.IActivate("player_state");
        }

        private void _DeSpawnPlayer()
        {
            _gameLoopManager.IDeActivate("player_state");
        }
    }
}
