using System.Collections.Generic;
using InputMainMenu;

namespace MyInput
{
    internal sealed class InputManager : IInputManager
    {
        private readonly IEventBus _bus;
        private readonly InputGroup _group;
        private readonly Dictionary<FactoryState.EState, IAction> _actions = new();

        public InputManager(IEventBus bus, InputGroup group)
        {
            _bus = bus;
            _group = group;

            IAction action = new ActionMainMenuState(_bus, _group);
            _actions[FactoryState.EState.eMainMenuState] = action;

            BindAction();
            CallbackAction();
        }

        private void BindAction()
        {
            if (_group.action == null) return;

            foreach (var item in _actions.Values)
            {
                item.IBindAction();
            }
        }

        private void CallbackAction()
        {
            foreach (var item in _actions.Values)
            {
                item.ICallbackAction();
            }
        }
    }
}

