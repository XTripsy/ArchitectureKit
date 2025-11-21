using System;
using System.Collections.Generic;

public readonly struct RequestStateEnter : IEvent
{
    public readonly string id;
    public RequestStateEnter(string id) 
        => this.id = id;
}

public class StateRegistry : IStateRegistry
{
    private readonly Dictionary<string, IState> _map = new();

    public IState ICreate(string id)
        => _map.TryGetValue(id, out var f) ? f : null;

    public void IRegister(string id, IState factory)
        => _map[id] = factory;
}