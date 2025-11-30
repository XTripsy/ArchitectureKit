using System.Collections.Generic;
using UnityEngine;

namespace Namespace_PlayerState
{
    internal sealed class PlayerStateMachine : IPlayerStateMachine
    {
        private Dictionary<string, IPlayerState> _playerStates = new();
        private IPlayerState _currentState;
        private string _currentStateName;

        public void IRegister(string name, IPlayerState state)
        {
            _playerStates[name] = state;
        }

        public void IUnregister(string name)
        {
            _playerStates.Remove(name);
        }

        public void IChangeState(string name)
        {
            if (!_playerStates.ContainsKey(name) || _currentState == _playerStates[name]) return;

            _currentState?.IExit();
            _currentState = _playerStates[name];
            _currentState.IEnter();
            _currentStateName = name;
        }

        public void IUpdate(float deltatime)
        {
            _currentState.IUpdate(deltatime);
        }
    }
}
