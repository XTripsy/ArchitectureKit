using UnityEngine;
using Namespace_PlayerState_Event;
using System.Collections.Generic;

namespace Namespace_PlayerState_Event
{
    internal readonly struct ActionPlayer : IEvent { }
}

namespace Namespace_PlayerState
{
    public sealed class PlayerStateManager : IUpdateManager, IPlayerStateManager
    {
        private Dictionary<string, IPlayerStateMachine> _stateMachine = new();

        public void IStart()
        {
            Debug.Log("Start Player State Manager");
        }

        public void IExit()
        {
            Debug.Log("Exit Player State Manager");
        }

        public void IUpdate(float deltatime)
        {
            foreach (var item in _stateMachine.Values)
            {
                item.IUpdate(deltatime);
            }
        }

        public void IAddStateMachine(string name, IPlayerStateMachine stateMachine)
        {
            _stateMachine[name] = stateMachine;
        }

        public void IRemoveStateMachine(string name)
        {
            _stateMachine.Remove(name);
        }
    }
}