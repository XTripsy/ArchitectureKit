using UnityEngine;

public sealed class PlayerStateManager : IUpdateManager
{
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
        Debug.Log("Update Player State Manager");
    }
}