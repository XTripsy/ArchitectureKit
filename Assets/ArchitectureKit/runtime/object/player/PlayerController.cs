using Namespace_PlayerModulMovement_Event;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Namespace_PlayerController
{
    internal sealed class PlayerController
    {
        private int _id;
        private Transform _transform;
        private PlayerInput _playerInput;
        private CharacterController _characterController;
        private IEventBus _bus;
        private IPlayerStateMachine _playerStateMachine;
        private Dictionary<string, InputAction> _actions = new();

        public PlayerController(int id, Transform transform, IEventBus bus, IPlayerStateMachine playerStateMachine)
        {
            _id = id;
            _transform = transform;
            _playerInput = _transform.GetComponent<PlayerInput>();
            _characterController = _transform.GetComponent<CharacterController>();
            _bus = bus;
            _playerStateMachine = playerStateMachine;

            _actions["movement"] = _playerInput.actions["action_movement"];

            _actions["movement"].performed += OnMove;
            _actions["movement"].canceled += OnMove;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 val = context.ReadValue<Vector2>();

            _bus.IPublish(new ActionPlayerMovement(_id, val));

            string state = (val.magnitude > 0)? "movement" : "idle";
            _playerStateMachine.IChangeState(state);
        }
    }
}
