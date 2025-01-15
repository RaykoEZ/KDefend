using System;
using UnityEngine;
using UnityEngine.Events;
// Concrete timer event
[Serializable]
public struct KDefenderTimerEvent : ITimerEvent<KDefenderGameState>
{
    [SerializeField] bool m_triggersOnce;
    [SerializeField] int m_triggerTime;
    [SerializeField] UnityEvent<KDefenderGameState> m_toInvoke;
    public UnityEvent<KDefenderGameState> ToInvoke => m_toInvoke;
    public int TriggerTime => m_triggerTime;
    public bool TriggersOnce => m_triggersOnce;
}
