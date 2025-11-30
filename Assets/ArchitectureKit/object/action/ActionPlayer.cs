using Namespace_ActionSpawnPlayer_Event;
using Namespace_PlayerState;

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
            //_bus.ISubscribe<ActionDeSpawnPlayer>(_ => _DeSpawnPlayer());
        }

        private void _SpawnPlayer()
        {
            _objectManager.IActive("player");
            //_gameLoopManager.IActivate("player_state_manager");
        }

        private void _DeSpawnPlayer()
        {
            _objectManager.IDeActive("player");
            //_gameLoopManager.IDeActivate("player_state_manager");
        }
    }
}
