public interface IPlayerStateManager
{
    void IAddStateMachine(string name, IPlayerStateMachine stateMachine);
    void IRemoveStateMachine(string name);
}