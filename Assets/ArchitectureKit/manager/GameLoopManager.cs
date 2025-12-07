using System.Collections.Generic;
using UnityEngine;

namespace Namespace_GameLoop
{
    internal sealed class GameLoopManager : IGameLoopManager
    {
        private Dictionary<string, IUpdateManager> _listUpdateManagers = new();
        private Dictionary<string, IUpdateManager> _activeUpdateManagers = new();

        public void IRegister(string name, IUpdateManager updateManager)
        {
            if (_listUpdateManagers.ContainsKey(name)) return;

            _listUpdateManagers[name] = updateManager;
        }

        public void IActivate(string name)
        {
            if (_activeUpdateManagers.ContainsKey(name)) return;

            IUpdateManager temp = _listUpdateManagers[name];
            _activeUpdateManagers[name] = temp;
            _activeUpdateManagers[name].IStart();
        }

        public void IDeActivate(string name)
        {
            if (!_activeUpdateManagers.ContainsKey(name)) return;

            _activeUpdateManagers[name].IExit();
            _activeUpdateManagers.Remove(name);
        }

        public IUpdateManager IGetManager(string name)
        {
            if (!_listUpdateManagers.ContainsKey(name)) return null;

            return _listUpdateManagers[name];
        }

        public void IUpdate()
        {
            if (_activeUpdateManagers.Count == 0) return;
            foreach (var item in _activeUpdateManagers.Values)
            {
                item.IUpdate(Time.deltaTime);
            }
        }
    }
}
