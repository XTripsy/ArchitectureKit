using UnityEngine;

namespace Namespace_PlayerModulMovement_Event
{
    internal readonly struct ActionPlayerMovement : IEvent 
    {
        public readonly int playerId;
        public readonly Vector2 dir;

        public ActionPlayerMovement(int playerId, Vector2 dir)
        {
            this.playerId = playerId;
            this.dir = dir;
        }
    }
}

namespace Namespace_PlayerModulMovement
{
    internal sealed class PlayerModulMovement
    {
        private readonly CharacterController _characterController;
        public Vector2 direction;
        public float speed = .1f;

        public PlayerModulMovement(CharacterController characterController)
        {
            _characterController = characterController;
        }

        public void UpdateMovement(float deltatime)
        {
            direction *= speed;
            Vector3 dir = new Vector3(direction.x, 0, direction.y);
            _characterController.Move(dir);
        }
    }
}
