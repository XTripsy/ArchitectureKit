using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Namespace_Player
{
    public struct PlayerComponents
    {
        public int id;
        public InputDevice device;
        public GameObject obj;
    }

    internal sealed class PlayerManager : IPlayerManager
    {
        private readonly IGameLoopManager _gameLoopManager;
        private List<PlayerComponents> _components = new();

        public PlayerManager(IGameLoopManager gameLoopManager)
        {
            _gameLoopManager = gameLoopManager;
        }

        public void IAddPlayer(PlayerComponents playerComponents)
        {
            _components.Add(playerComponents);
        }

        public void IRemoveAllPlayer()
        {
            IPlayerStateManager playerstate = _gameLoopManager.IGetManager("player_state_manager") as IPlayerStateManager;
            int id;

            for (int i = 0; i < _components.Count; i++)
            {
                id = _components[i].id;
                GameObject player = _GetObjectID(id);
                playerstate.IRemoveStateMachine("player_state-" + id);
                GameObject.Destroy(player);
            }
        }

        public void IRemovePlayer(int id)
        {
            IPlayerStateManager playerstate = _gameLoopManager.IGetManager("player_state_manager") as IPlayerStateManager;
            GameObject player = _GetObjectID(id);

            PlayerComponents components = _components.Find(item => item.id == id);
            _components.Remove(components);

            playerstate.IRemoveStateMachine("player_state-" + id);
            GameObject.Destroy(player);
        }

        public bool IIsCanSpawn(InputDevice device)
        {
            bool isDeviceRegistered = _components.Any(player => player.device == device);
            return !isDeviceRegistered;
        }

        private GameObject _GetObjectID(int id)
        {
            PlayerComponents result = _components.Find(item => item.id == id);
            return result.obj;
        }
    }
}
