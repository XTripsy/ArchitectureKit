using System.Collections.Generic;
using UnityEngine;

public interface IUIManager
{
    void IShow(string name);
    void IHide(string name);
    GameObject IGet(string name);
    T IGetComponentInUI<T>(string name, string childPath) where T : Component;
    public IEnumerable<T> IGetAllComponentInUI<T>(string uiName) where T : Component;
}