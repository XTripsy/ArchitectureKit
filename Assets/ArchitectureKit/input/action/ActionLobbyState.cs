using System.Collections.Generic;
using UnityEngine.InputSystem;

using Namespace_StateLobby_Event;
using Namespace_InputLobby_Event;

namespace Namespace_InputLobby
{
    internal sealed class ActionLobbyState : IAction
    {
        private readonly IEventBus _bus;
        private readonly InputActionMap _inputActions;
        private readonly InputCatalog.Mapping _mapping;

        private Dictionary<string, InputAction> _aActions = new();

        public ActionLobbyState(IEventBus bus, InputActionMap inputActions, InputCatalog.Mapping mapping)
        {
            _bus = bus;
            _mapping = mapping;
            _inputActions = inputActions;

            _bus.ISubscribe<LobbyStateEnter>(_ => IEnable());
        }

        public void IBindAction()
        {
            foreach (var item in _mapping.actions)
                _aActions[item] = _inputActions.FindAction(item, false);
        }

        public void ICallbackAction()
        {
            _aActions["action_join"].started += ctx =>
            {
                var device = ctx.control?.device;
                _bus.IPublish(new ActionJoinLobbyState(device));
            };
        }

        public void IDisable()
            => _inputActions.Disable();

        public void IEnable()
            => _inputActions.Enable();
    }
}
