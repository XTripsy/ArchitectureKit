using UnityEngine;

public readonly struct ActionClickGameplayState : IEvent { }
public sealed class InputActionGameplayState
{
    IEventBus _bus;

    public InputActionGameplayState(IEventBus bus)
    { 
        _bus = bus; 
    }

    public void ClickGameplay()
    {
        Debug.LogError("CLICK");
    }
}