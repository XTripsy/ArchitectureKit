using UnityEngine;
using System.Collections.Generic;

namespace Namespace_Trigger
{
    internal sealed class TriggerManager : ITriggerManager
    {
        private readonly IEventBus _bus;
        private Dictionary<string, ITriggerSystem> _triggerSystem = new();

        public TriggerManager(IEventBus bus)
        {
            _bus = bus;

            _bus.ISubscribe<TriggerRegister>(TriggerRegister);
        }

        private void TriggerRegister(TriggerRegister trigger)
        {
            List<ITriggerPoint> points = new();

            foreach (var item in trigger.root)
            {
                if (!item.TryGetComponent<ITriggerPoint>(out ITriggerPoint point)) continue;

                points.Add(point);
            }

            foreach (var point in points)
            {
                point.IInitTrigger(_bus);
            }
        }

        public void IAddTriggerSystem(string name, ITriggerSystem triggerSystem)
        {
            _triggerSystem[name] = triggerSystem;
        }

        public void IRemoveTriggerSystem(string name)
        {
            _triggerSystem.Remove(name);
        }
    }
}
