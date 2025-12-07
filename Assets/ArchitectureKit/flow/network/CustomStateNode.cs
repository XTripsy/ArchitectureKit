
using PurrLobby;
using PurrNet;
using PurrNet.StateMachine;

public abstract class CustomStateNode : StateNode
{
    public virtual void Init(IEventBus bus, LobbyManager lobbyManager) { }
}


public abstract class CustomStateNode<T> : CustomStateNode
{

    // public virtual void Init(IEventBus bus, LobbyManager lobbyManager) { }

    /// <summary>
    /// This is called when the state is entered.
    /// </summary>
    /// <param name="data">The data which the state is entered with</param>
    public virtual void Enter(T data)
    {
    }

    /// <summary>
    /// This is called when the state is entered.
    /// </summary>
    /// <param name="asServer">Whether you are acting as server or client</param>
    /// <param name="data">The data which the state is entered with</param>
    public virtual void Enter(T data, bool asServer)
    {
    }

    /// <summary>
    /// Override this to control whether the state can be entered
    /// </summary>
    /// <param name="data">The data which the state is attempted to be entered with</param>
    public virtual bool CanEnter(T data) => true;
}
