public readonly struct MainMenuStateEnter : IEvent { }
public readonly struct MainMenuStateExit : IEvent { }

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
}
