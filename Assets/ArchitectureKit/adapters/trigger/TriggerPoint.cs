using UnityEngine;

public sealed class TriggerPoint : MonoBehaviour, ITriggerPoint
{
    [SerializeField] private string _nameEvent;
    private IEventBus _bus;

    public void IInitTrigger(IEventBus bus)
    {
        _bus = bus;
        Debug.LogError("MASUKKK PEPEK");
    }

    private void OnTriggerEnter(Collider other)
    {
        _bus?.IPublish(new TriggerEnter(_nameEvent, other));
    }

    private void OnTriggerExit(Collider other)
    {
        _bus?.IPublish(new TriggerExit(_nameEvent, other)); 
    }
}