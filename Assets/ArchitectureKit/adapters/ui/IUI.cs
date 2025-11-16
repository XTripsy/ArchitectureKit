using UnityEngine;

public interface IUI
{
    void IShow(string uiName); 
    void IHide(string uiName);
    GameObject IGet(string uiName);
    T IGetComponentInUI<T>(string uiName, string childPath) where T : Component;
}