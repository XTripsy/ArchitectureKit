using UnityEngine.InputSystem;
using System.Collections.Generic;
using Namespace_StatePause_Event;
using Namespace_InputPause_Event;

namespace Namespace_InputPause
{
    internal sealed class ActionPauseState : IAction
    {
        private readonly IEventBus _bus;
        private readonly InputActionMap _inputActions;
        private readonly InputCatalog.Mapping _mapping;

        private Dictionary<string, InputAction> _aActions = new();

        public ActionPauseState(IEventBus bus, InputActionMap inputActions, InputCatalog.Mapping mapping)
        {
            _bus = bus;
            _mapping = mapping;
            _inputActions = inputActions;

            _bus.ISubscribe<PauseStateEnter>(_ => IEnable());
        }

        public void IBindAction()
        {
            foreach (var item in _mapping.actions)
                _aActions[item] = _inputActions.FindAction(item, false);
        }

        public void ICallbackAction()
        {
            _aActions["action_resume"].started += _ => _bus.IPublish(new ActionResumePauseState());
        }

        public void IDisable()
            => _inputActions.Disable();

        public void IEnable()
            => _inputActions.Enable();
    }
}
