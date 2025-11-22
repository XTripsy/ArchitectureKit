using UnityEngine.InputSystem;
using Namespace_StatePause_Event;
using Namespace_InputPause_Event;

namespace Namespace_InputPause
{
    internal sealed class ActionPauseState : IAction
    {
        private readonly IEventBus _bus;
        private readonly InputActionMap _inputActions;
        private readonly InputCatalog.Mapping _mapping;

        private InputAction _aResume;

        public ActionPauseState(IEventBus bus, InputActionMap inputActions, InputCatalog.Mapping mapping)
        {
            _bus = bus;
            _mapping = mapping;
            _inputActions = inputActions;

            _bus.ISubscribe<PauseStateEnter>(_ => IEnable());
        }

        public void IBindAction()
        {
            _aResume = _inputActions.FindAction(_mapping.actions[0], false);
        }

        public void ICallbackAction()
        {
            _aResume.started += _ => _bus.IPublish(new ActionResumePauseState());
        }

        public void IDisable()
            => _inputActions.Disable();

        public void IEnable()
            => _inputActions.Enable();
    }
}
