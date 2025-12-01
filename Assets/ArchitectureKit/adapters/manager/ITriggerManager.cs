public interface ITriggerManager
{
    void IAddTriggerSystem(string name, ITriggerSystem triggerSystem);
    void IRemoveTriggerSystem(string name);
}