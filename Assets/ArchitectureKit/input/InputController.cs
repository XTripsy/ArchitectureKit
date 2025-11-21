using UnityEngine;

public sealed class InputController
{
    private readonly IEventBus _bus;

    public InputController(IEventBus bus)
    {
        _bus = bus;

        /*InputActionMainMenuState temp = new InputActionMainMenuState(_bus);
        _bus.ISubscribe<ActionPlayMainMenuState>(_ => temp.PlayMainMenu());*/
    }
}
