using UnityEngine;

public interface IUIManager
{
    void IShow(string uiName); 
    void IHide(string uiName);
    GameObject IGet(string uiName);
    T IGetComponentInUI<T>(string uiName, string childPath) where T : Component;
}