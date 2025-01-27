using System;
using UnityEngine;
using UnityEngine.Events;
// Concrete timer event
[Serializable]
public struct KDefenderTimerEvent : ITimerEvent<KDefenderEventContext>
{
    [SerializeField] int m_triggerTime;
    [SerializeField] UnityEvent<KDefenderEventContext> m_toInvoke;
    public UnityEvent<KDefenderEventContext> ToInvoke => m_toInvoke;
    public int TriggerTime => m_triggerTime;
}