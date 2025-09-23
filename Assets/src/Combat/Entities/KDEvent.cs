using Curry.Events;
using System;
using UnityEngine;


[Serializable]
public abstract class KDEvent 
{
    [SerializeField] protected KD_StaticEventFlags m_triggerConditions = default;
    [SerializeField] protected KD_StaticEventFlags m_raiseOnTrigger = default;
    [SerializeField] protected GameEventTriggerType m_triggerType = default;
    [SerializeField] protected GameEventTriggerType m_triggerTypeOnRaise = default;
    protected KDGameEventTrigger ToInvoke => OnEventTrigger;
    public KDEvent(KD_StaticEventFlags triggerConditions, KD_StaticEventFlags raiseOnTigger,
    GameEventTriggerType triggerType, GameEventTriggerType raiseTriggerType)
    {
        m_triggerConditions = triggerConditions;
        m_raiseOnTrigger = raiseOnTigger;
        m_triggerType = triggerType;
        m_triggerTypeOnRaise = raiseTriggerType;
    }
    public void InitGlobalListeners() 
    {
        InternalEventHandler.ListenToGlobal(m_triggerType, ToInvoke);
    }
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
        // raise flag & chain events if valid, pass on new flag
        if (m_raiseOnTrigger != KD_StaticEventFlags.None) 
        {
            InternalEventHandler.TriggerGlobalEvent(this, 
                new KDEventInfo(m_raiseOnTrigger, m_triggerTypeOnRaise, args.Payload));
        }
    }
    protected abstract void Trigger_Internal(object sender, KDEventInfo args);
}