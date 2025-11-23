using Namespace_InputGameplay_Event;
using Namespace_StateGameplay_Event;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Namespace_InputGameplay
{
    internal sealed class ActionGameplayState : IAction
    {
        private readonly IEventBus _bus;
        private readonly InputActionMap _inputActions;
        private readonly InputCatalog.Mapping _mapping;

        private Dictionary<string, InputAction> _aActions = new();

        public ActionGameplayState(IEventBus bus, InputActionMap inputActions, InputCatalog.Mapping mapping)
        {
            _bus = bus;
            _mapping = mapping;
            _inputActions = inputActions;

            _bus.ISubscribe<GameplayStateEnter>(_ => IEnable());
        }

        public void IBindAction()
        {
            foreach (var item in _mapping.actions)
                _aActions[item] = _inputActions.FindAction(item, false);
        }

        public void ICallbackAction()
        {
            _aActions["action_click"].started += _ => _bus.IPublish(new ActionClickGameplayState());
            _aActions["action_pause"].started += _ => _bus.IPublish(new ActionPauseGameplayState());
        }

        public void IDisable()
            => _inputActions.Disable();

        public void IEnable()
            => _inputActions.Enable();
    }
}
