using UnityEngine;

public struct TriggerRegister : IEvent
{
    public readonly GameObject[] root;

    public TriggerRegister(GameObject[] _root)
    {
        root = _root;
    }
}

public struct TriggerEnter : IEvent
{
    public readonly string nameEvent;
    public readonly Collider collider;

    public TriggerEnter(string _nameEvent, Collider _collider)
    {
        nameEvent = _nameEvent;
        collider = _collider;
    }
}

public struct TriggerExit : IEvent
{
    public readonly string nameEvent;
    public readonly Collider collider;

    public TriggerExit(string _nameEvent, Collider _collider)
    {
        nameEvent = _nameEvent;
        collider = _collider;
    }
}