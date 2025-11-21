using System.Collections.Generic;
using InputMainMenu;
using UnityEngine;

namespace MyInput
{
    internal sealed class InputManager : IInputManager
    {
        private Dictionary<string, IAction> _actions = new();

        public void IRegisterActionInput(string state, IAction action)
        {
            _actions[state] = action;
            _actions[state]?.IBindAction();
            _actions[state]?.ICallbackAction();
        }

        public void IActiveActionInput(string mapping)
        {
            foreach (var item in _actions.Values)
                item.IDisable();

            _actions[mapping]?.IEnable();
        }

        public int IGetIndexCatalogInputAction(string state, InputCatalog group)
        {
            int id = 0;
            foreach(var item in group.InputAction)
            {
                if (item.nameMapping == state)
                    return id;
                id++;
            }

            return -1;
        }
    }
}