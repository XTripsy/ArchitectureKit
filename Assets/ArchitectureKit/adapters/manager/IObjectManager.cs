using UnityEngine;

public interface IObjectManager
{
    void IActive(string name);
    void IDeActive(string name);
    GameObject IGet(string name);
    T IGetComponentInObject<T>(string name, string childPath) where T : Component;
    GameObject IDuplicateSpawn(string name, int id);
}