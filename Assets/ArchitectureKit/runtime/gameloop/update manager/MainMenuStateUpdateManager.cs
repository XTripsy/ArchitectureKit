using UnityEngine;

namespace Namespace_StateMainMenu
{
    public sealed class MainMenuStateUpdateManager : IUpdateManager
    {
        public void IStart()
        {
            Debug.Log("Start Main Menu State Manager");
        }

        public void IExit()
        {
            Debug.Log("Exit Main Menu State Manager");
        }

        public void IUpdate(float deltatime)
        {
            Debug.Log("Update Main Menu State Manager");
        }
    }
}