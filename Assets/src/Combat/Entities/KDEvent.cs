using Curry.Events;
using UnityEngine;
public abstract class KDEvent 
{
    [SerializeField] protected KD_StaticEventFlags m_triggerConditions = default;
    [SerializeField] protected KD_StaticEventFlags m_raiseOnTrigger = default;
    [SerializeField] protected GameEventTriggerType m_triggerType = default;
    [SerializeField] protected GameEventTriggerType m_triggerTypeOnRaise = default;
    public KDEvent(KD_StaticEventFlags triggerConditions, KD_StaticEventFlags raiseOnTigger, 
        GameEventTriggerType triggerType, GameEventTriggerType raiseTriggerType)
    {
        m_triggerConditions = triggerConditions;
        m_raiseOnTrigger = raiseOnTigger;
        m_triggerType = triggerType;
        m_triggerTypeOnRaise = raiseTriggerType;
    }
    protected KDGameEventTrigger ToInvoke => OnEventTrigger;
    // frontend call for event trigger 
    protected void OnEventTrigger(object sender, KDEventInfo args) 
    {
        if (args == null) return;
        // check condition flag
        bool fitsFlags = (args.Flags & m_triggerConditions) == m_triggerConditions;
        if (!fitsFlags) return;
        // do own stuff here
        Trigger_Internal(sender, args);
        // call this on finish
        args?.OnFinishedCallback?.Invoke();
        // raise flag & chain events if valid
        if (m_raiseOnTrigger != KD_StaticEventFlags.None) 
        {
            InternalEventHandler.TriggerGlobalEvent(this, 
                new KDEventInfo(args.Flags | m_raiseOnTrigger, m_triggerTypeOnRaise, args.Payload));
        }
    }
    protected abstract void Trigger_Internal(object sender, KDEventInfo args);
}