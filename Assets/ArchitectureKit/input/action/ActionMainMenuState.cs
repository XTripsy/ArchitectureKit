using UnityEngine.InputSystem;
using Namespace_StateMainMenu_Event;
using Namespace_InputMainMenuEvent;

namespace Namespace_InputMainMenu
{
    internal sealed class ActionMainMenuState : IAction
    {
        private readonly IEventBus _bus;
        private readonly InputActionMap _inputActions;
        private readonly InputCatalog.Mapping _mapping;

        private InputAction _aPlay;

        public ActionMainMenuState(IEventBus bus, InputActionMap inputActions, InputCatalog.Mapping mapping)
        {
            _bus = bus;
            _mapping = mapping;
            _inputActions = inputActions;

            _bus.ISubscribe<MainMenuStateEnter>(_ => IEnable());
        }

        public void IBindAction()
        {
            _aPlay = _inputActions.FindAction(_mapping.actions[0], false);
        }

        public void ICallbackAction()
        {
            _aPlay.started += _ => _bus.IPublish(new ActionPlayMainMenuState());
        }

        public void IDisable()
            => _inputActions.Disable();

        public void IEnable()
            => _inputActions.Enable();
    }
}
