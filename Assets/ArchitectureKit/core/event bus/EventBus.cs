using System;
using System.Collections.Generic;

public sealed class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> _events = new();

    public void IPublish<T>(T e) where T : IEvent
    {
        if (!_events.TryGetValue(typeof(T), out var list)) return;
        
        foreach (var d in list.ToArray()) ((System.Action<T>)d)?.Invoke(e);
    }

    public void ISubscribe<T>(System.Action<T> h) where T : IEvent
    {
        if (!_events.TryGetValue(typeof(T), out var list)) _events[typeof(T)] = list = new();
        list.Add(h);
    }

    public void IUnsubscribe<T>(System.Action<T> h) where T : IEvent
    {
        if (_events.TryGetValue(typeof(T), out var list)) list.Remove(h);
    }
}
