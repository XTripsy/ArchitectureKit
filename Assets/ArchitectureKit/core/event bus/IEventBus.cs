using System;

public interface IEventBus
{
    void IPublish<T>(T e) where T : IEvent;
    void ISubscribe<T>(Action<T> h) where T : IEvent;
    void IUnsubscribe<T>(Action<T> h) where T : IEvent;
}
