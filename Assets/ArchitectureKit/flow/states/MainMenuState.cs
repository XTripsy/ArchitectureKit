public readonly struct RequestMainMenuStateEnter : IEvent { }
public readonly struct RequestMainMenuStateExit : IEvent { }
public readonly struct RequestMainMenuStateUpdate : IEvent { }

public readonly struct MainMenuStateEnter : IEvent { }
public readonly struct MainMenuStateExit : IEvent { }
public readonly struct MainMenuStateUpdate : IEvent { }

public sealed class MainMenuState : IState
{
    private readonly IEventBus _bus;

    public MainMenuState(IEventBus temp_bus)
    {
        _bus = temp_bus;
    }

    public void IOnEnter()
    {
        _bus.IPublish(new MainMenuStateEnter());
    }

    public void IOnExit()
    {
        _bus.IPublish(new MainMenuStateExit());
    }

    public void IOnUpdate(float deltatime)
    {
        _bus.IPublish(new MainMenuStateUpdate());
    }
}
