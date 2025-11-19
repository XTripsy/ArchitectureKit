using UnityEngine;

public interface IFactory<out T>
{
    T Create();
}

public interface IKeyFactory<in TKey, out T>
{
    T Create(TKey key);
}