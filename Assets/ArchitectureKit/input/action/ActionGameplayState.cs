using UnityEngine.InputSystem;
using Namespace_StateGameplay_Event;
using Namespace_InputGameplay_Event;

namespace Namespace_InputGameplay
{
    internal sealed class ActionGameplayState : IAction
    {
        private readonly IEventBus _bus;
        private readonly InputActionMap _inputActions;
        private readonly InputCatalog.Mapping _mapping;

        private InputAction _aClick;
        private InputAction _aPause;

        public ActionGameplayState(IEventBus bus, InputActionMap inputActions, InputCatalog.Mapping mapping)
        {
            _bus = bus;
            _mapping = mapping;
            _inputActions = inputActions;

            _bus.ISubscribe<GameplayStateEnter>(_ => IEnable());
        }

        public void IBindAction()
        {
            _aClick = _inputActions.FindAction(_mapping.actions[0], false);
            _aPause = _inputActions.FindAction(_mapping.actions[1], false);
        }

        public void ICallbackAction()
        {
            _aClick.started += _ => _bus.IPublish(new ActionClickGameplayState());
            _aPause.started += _ => _bus.IPublish(new ActionPauseGameplayState());
        }

        public void IDisable()
            => _inputActions.Disable();

        public void IEnable()
            => _inputActions.Enable();
    }
}
