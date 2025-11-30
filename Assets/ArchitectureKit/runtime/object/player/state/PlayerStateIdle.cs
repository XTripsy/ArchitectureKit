using Namespace_PlayerModulMovement;
using UnityEngine;

namespace Namespace_PlayerState
{
    internal sealed class PlayerStateIdle : IPlayerState
    {
        private readonly PlayerModulMovement _playerModulMovement;

        public PlayerStateIdle(PlayerModulMovement playerModulMovement)
        {
            _playerModulMovement = playerModulMovement;
        }

        public void IEnter()
        {
            Debug.LogError("ENTER IDLE");
        }

        public void IExit()
        {
            Debug.LogError("EXIT IDLE");
        }

        public void IUpdate(float deltatime)
        {
            _playerModulMovement.direction = Vector2.zero;
            _playerModulMovement.UpdateMovement(deltatime);
        }
    }
}
