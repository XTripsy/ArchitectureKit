public interface IPlayerStateMachine
{
    void IRegister(string name, IPlayerState state);
    void IUnregister(string name);
    void IChangeState(string name);
    void IUpdate(float deltatime);
}