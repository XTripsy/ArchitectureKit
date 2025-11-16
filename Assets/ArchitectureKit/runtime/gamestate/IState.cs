public interface IState
{
    void IOnEnter();
    void IOnExit();
    void IOnUpdate(float deltatime);
}
