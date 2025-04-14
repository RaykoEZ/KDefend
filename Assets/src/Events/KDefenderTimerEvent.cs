using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Concrete timer event
[Serializable]
public class KDefenderTimerEvent : ITimerEvent<KDefenderEventContext>
{
    [SerializeField] protected int m_triggerTime;
    [SerializeField] UnityEvent<KDefenderEventContext> m_toInvoke;
    public UnityEvent<KDefenderEventContext> ToInvoke => m_toInvoke;
    public int TriggerTime => m_triggerTime;
}