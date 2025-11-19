using UnityEngine;

public class GameLoopManager : Manager
{
    public UpdateHandler updateHandler;

    public GameLoopManager()
    {

    }

    public void IStart()
    {
        
    }

    public void IUpdate()
    {
        updateHandler?.Update(Time.deltaTime);
    }
}
