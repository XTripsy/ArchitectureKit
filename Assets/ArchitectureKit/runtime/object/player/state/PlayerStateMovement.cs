using Namespace_PlayerModulMovement;
using Namespace_PlayerModulMovement_Event;
using UnityEngine;

namespace Namespace_PlayerState
{
    internal sealed class PlayerStateMovement : IPlayerState
    {
        private IEventBus _bus;
        private readonly PlayerModulMovement _playerModulMovement;
        private Vector2 _direction;

        public PlayerStateMovement(IEventBus bus, PlayerModulMovement playerModulMovement)
        {
            _bus = bus;
            _playerModulMovement = playerModulMovement;
            _bus.ISubscribe<ActionPlayerMovement>(GetMovement);
        }

        public void IEnter()
        {
            Debug.LogError("ENTER MOVE");
        }

        public void IExit()
        {
            Debug.LogError("EXIT MOVE");
        }

        public void IUpdate(float deltatime)
        {
            _playerModulMovement.direction = _direction;
            _playerModulMovement.UpdateMovement(deltatime);
        }

        private void GetMovement(ActionPlayerMovement action)
        {
            _direction = action.dir;
        }
    }
}
