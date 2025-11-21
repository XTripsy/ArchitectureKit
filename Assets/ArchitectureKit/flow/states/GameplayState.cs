public readonly struct GameplayStateEnter : IEvent { }
public readonly struct GameplayStateExit : IEvent { }

public sealed class GameplayState : IState
{
    private readonly IEventBus _bus;

    public GameplayState(IEventBus temp_bus)
    {
        _bus = temp_bus;
    }

    public void IOnEnter()
    {
        _bus.IPublish(new GameplayStateEnter());
    }

    public void IOnExit()
    {
        _bus.IPublish(new GameplayStateExit());
    }
}
