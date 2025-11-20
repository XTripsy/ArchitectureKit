using UnityEngine;

public interface IAction
{
    void IBindAction();
    void ICallbackAction();
    int IGetIndexInputAction(FactoryState.EState state);
}
