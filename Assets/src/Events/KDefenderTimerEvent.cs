using System;
using UnityEngine;
using UnityEngine.Events;
// Concrete timer event
[Serializable]
public struct KDefenderTimerEvent : ITimerEvent<GameEventContext>
{
    [SerializeField] int m_triggerTime;
    [SerializeField] UnityEvent<GameEventContext> m_toInvoke;
    public UnityEvent<GameEventContext> ToInvoke => m_toInvoke;
    public int TriggerTime => m_triggerTime;
}