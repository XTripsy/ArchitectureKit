using UnityEngine;

public interface IAction
{
    void IEnable();
    void IDisable();
    void IBindAction();
    void ICallbackAction();
    //int IGetIndexInputAction(string state);
}
