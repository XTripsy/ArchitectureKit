using Namespace_InputMainMenu_Event;
using Namespace_StateMainMenu_Event;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Namespace_InputMainMenu
{
    internal sealed class ActionMainMenuState : IAction
    {
        private readonly IEventBus _bus;
        private readonly InputActionMap _inputActions;
        private readonly InputCatalog.Mapping _mapping;

        private Dictionary<string, InputAction> _aActions = new();

        public ActionMainMenuState(IEventBus bus, InputActionMap inputActions, InputCatalog.Mapping mapping)
        {
            _bus = bus;
            _mapping = mapping;
            _inputActions = inputActions;

            _bus.ISubscribe<MainMenuStateEnter>(_ => IEnable());
        }

        public void IBindAction()
        {
            foreach (var item in _mapping.actions)
                _aActions[item] = _inputActions.FindAction(item, false);
        }

        public void ICallbackAction()
        {
            _aActions["action_play"].started += _ => _bus.IPublish(new ActionPlayMainMenuState());
        }

        public void IDisable()
            => _inputActions.Disable();

        public void IEnable()
            => _inputActions.Enable();
    }
}
