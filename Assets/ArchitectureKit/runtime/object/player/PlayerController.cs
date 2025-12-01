using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

using Namespace_PlayerModulMovement_Event;
using Namespace_PlayerController_Event;

namespace Namespace_PlayerController
{
    internal sealed class PlayerController
    {
        private int _id;
        private PlayerInput _playerInput;
        private IEventBus _bus;
        private IPlayerStateMachine _playerStateMachine;
        private Dictionary<string, InputAction> _actions = new();

        public PlayerController(int id, PlayerInput playerInput, IEventBus bus, IPlayerStateMachine playerStateMachine)
        {
            _id = id;
            _playerInput = playerInput;
            _bus = bus;
            _playerStateMachine = playerStateMachine;

            _actions["movement"] = _playerInput.actions["action_movement"];
            _actions["interact"] = _playerInput.actions["action_interact"];

            _actions["movement"].performed += OnMove;
            _actions["movement"].canceled += OnMove;

            _actions["interact"].performed += OnInteract;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 val = context.ReadValue<Vector2>();

            _bus.IPublish(new ActionPlayerMovement(_id, val));

            string state = (val.magnitude > 0)? "movement" : "idle";
            _playerStateMachine.IChangeState(state);
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            _bus.IPublish(new ActionPlayerInteract());
        }
    }
}
