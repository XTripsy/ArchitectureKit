public interface IPlayerState
{
    void IEnter();
    void IExit();
    void IUpdate(float deltatime);
}