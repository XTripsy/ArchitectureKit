using UnityEngine;

public readonly struct ActionPlayMainMenuState : IEvent { }
public sealed class InputActionMainMenuState
{
    IEventBus _bus;

    public InputActionMainMenuState(IEventBus bus)
    {
        _bus = bus;
    }

    public void PlayMainMenu()
    {
        _bus.IPublish(new SLevelRequest("gameplay_scene"));
        _bus.IPublish(new RequestGameplayStateEnter());
    }
}
