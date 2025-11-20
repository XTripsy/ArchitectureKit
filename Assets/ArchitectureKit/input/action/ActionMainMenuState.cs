using UnityEngine;
using UnityEngine.InputSystem;
using MyInput;

namespace InputMainMenu
{
    internal sealed class ActionMainMenuState : IAction
    {
        private readonly IEventBus _bus;
        private readonly InputGroup _group;

        private InputAction _aPlay;

        public ActionMainMenuState(IEventBus bus, InputGroup group)
        {
            _bus = bus;
            _group = group;

            _bus.ISubscribe<MainMenuStateEnter>(_ => EnableInput());
        }

        public void IBindAction()
        {
            int indexMapping = IGetIndexInputAction(FactoryState.EState.eMainMenuState);
            InputCatalog.Mapping mapping = _group.catalog.InputAction[indexMapping];
            var mapMainMenu = _group.action.FindActionMap(mapping.nameMapping, throwIfNotFound: false);

            _aPlay = mapMainMenu.FindAction(mapping.actions[0], false);
        }

        public void ICallbackAction()
        {
            _aPlay.started += _ => _bus.IPublish(new ActionPlayMainMenuState());
        }

        public int IGetIndexInputAction(FactoryState.EState state)
        {
            int index = 0;
            foreach (var action in _group.catalog.InputAction)
            {
                if (action.state == state)
                    return index;
                index++;
            }

            return -1;
        }

        private void DisableAll()
        {
            foreach (var map in _group.action.actionMaps) map.Disable();
        }

        private void EnableInput()
        {
            DisableAll();
            int indexMapping = IGetIndexInputAction(FactoryState.EState.eMainMenuState);
            string mapping = _group.catalog.InputAction[indexMapping].nameMapping;
            _group.action.FindActionMap(mapping, throwIfNotFound: false)?.Enable();
        }
    }
}
