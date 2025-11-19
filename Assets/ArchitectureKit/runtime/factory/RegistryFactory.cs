using System;
using System.Collections.Generic;

public sealed class RegistryFactory<TKey, T> : IKeyFactory<TKey, T>
{
    private readonly Dictionary<TKey, Func<T>> _map = new();
    public void Register(TKey key, Func<T> ctor) => _map[key] = ctor;
    public T Create(TKey key) => _map[key]();
}