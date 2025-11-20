using UnityEngine;

namespace MyGameLoop
{
    internal class GameLoopManager : IGameLoopManager
    {
        private readonly UpdateHandler _updateHandler;

        public GameLoopManager(UpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;
        }

        public void IUpdate()
        {
            _updateHandler?.Update(Time.deltaTime);
        }
    }
}
