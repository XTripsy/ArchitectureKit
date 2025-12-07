using Namespace_ActionSpawnPlayer_Event;
using Namespace_ExitLobby_Event;
using UnityEngine;

namespace Namespace_ActionSpawnPlayer_Event
{
    internal readonly struct ActionSpawnPlayer : IEvent { }
    internal readonly struct ActionDeSpawnPlayer : IEvent { }
}

namespace Namespace_ActionSpawnPlayer
{
    internal sealed class ActionPlayer
    {
        private IEventBus _bus;
        private IGameLoopManager _gameLoopManager;

        public ActionPlayer(IEventBus bus, IGameLoopManager gameLoopManager)
        {
            _bus = bus;
            _gameLoopManager = gameLoopManager;

            _bus.ISubscribe<ActionSpawnPlayer>(_ => _SpawnPlayer());
            _bus.ISubscribe<ExitLobbyPlayer>(_DeSpawnPlayer);
        }

        private void _SpawnPlayer()
        {
            _gameLoopManager.IActivate("player_state_manager");
        }

        private void _DeSpawnPlayer(ExitLobbyPlayer player)
        {
            string name = player.collider.transform.name;
            int index = name.LastIndexOf('-');
            string id = name.Substring(index + 1);

            IPlayerStateManager temp = _gameLoopManager.IGetManager("player_state_manager") as IPlayerStateManager;
            temp.IRemoveStateMachine("player_state-" + id);
            GameObject.Destroy(player.collider.gameObject);
        }
    }
}
