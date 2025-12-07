public interface IGameLoopManager
{
    void IRegister(string name, IUpdateManager updateManager);
    void IActivate(string name);
    void IDeActivate(string name);
    IUpdateManager IGetManager(string name);
    void IUpdate();
}
