using UnityEngine;

public class GameLoopManager : Manager
{
    public UpdateHandler updateHandler;
    private void Update()
    {
        updateHandler?.Update(Time.deltaTime);
    }
}
