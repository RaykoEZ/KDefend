using Curry.Events;
using System;
using UnityEngine;


[Serializable]
public abstract class KDEvent 
{
    [SerializeField] protected KD_StaticEventFlags m_flagConditionsToTrigger = default;
    [SerializeField] protected GameEventTriggerType m_triggerTypeToListen = default;
    [SerializeField] protected KD_StaticEventFlags m_raiseFlagOnTrigger = default;
    [SerializeField] protected GameEventTriggerType m_triggerTypeOnRaise = default;
    protected KDGameEventTrigger ToInvoke => OnEventTrigger;
    public KDEvent(KD_StaticEventFlags triggerConditions, KD_StaticEventFlags raiseOnTigger,
    GameEventTriggerType triggerType, GameEventTriggerType raiseTriggerType)
    {
        m_flagConditionsToTrigger = triggerConditions;
        m_raiseFlagOnTrigger = raiseOnTigger;
        m_triggerTypeToListen = triggerType;
        m_triggerTypeOnRaise = raiseTriggerType;
    }
    public void InitGlobalListeners() 
    {
        InternalEventHandler.ListenToGlobal(m_triggerTypeToListen, ToInvoke);
    }
    // frontend call for event trigger 
    protected void OnEventTrigger(object sender, KDEventInfo args) 
    {
        if (args == null) return;
        // check condition flag
        bool fitsFlags = (args.Flags & m_flagConditionsToTrigger) == m_flagConditionsToTrigger;
        if (!fitsFlags) return;
        // do own stuff here
        Trigger_Internal(sender, args);
        // call this on finish
        args?.OnFinishedCallback?.Invoke();
        // raise flag & chain events if valid, pass on new flag
        if (m_raiseFlagOnTrigger != KD_StaticEventFlags.None) 
        {
            InternalEventHandler.TriggerGlobalEvent(this, 
                new KDEventInfo(m_raiseFlagOnTrigger, m_triggerTypeOnRaise, args.Payload));
        }
    }
    protected abstract void Trigger_Internal(object sender, KDEventInfo args);
}