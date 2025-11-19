using UnityEngine;

public interface IFactory<out T>
{
    T Create();
}

public interface IFactory<in TArg, out T>
{
    T Create(TArg arg);
}

public interface IKeyFactory<in TKey, out T>
{
    T Create(TKey key);
}