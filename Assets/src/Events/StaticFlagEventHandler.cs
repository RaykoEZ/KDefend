using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StaticFlagEventHandler: MonoBehaviour 
{
    [Serializable]
    public struct FlagEventItem
    {
        public KD_StaticEventFlags TriggerCondition;
        public UnityEvent<SaveData> ToTrigger;
    }
    [SerializeField] GameSaveSource m_state = default;
    [SerializeField] List<FlagEventItem> m_toTrigger = default;
    KD_StaticEventFlags m_currentFlags = KD_StaticEventFlags.None;
    public KD_StaticEventFlags CurrentFlags => m_currentFlags;
    public void SetFlags(KD_StaticEventFlags newFlags) 
    {
        bool conditionMatches;
        bool alreadyTriggered;
        KD_StaticEventFlags prev = m_currentFlags;
        m_currentFlags = newFlags;
        // Auto Save here
        m_state.UpdateSave(true);
        foreach (var item in m_toTrigger)
        {
            conditionMatches = (item.TriggerCondition & m_currentFlags) > 0;
            // check if old flags already
            alreadyTriggered = (item.TriggerCondition & prev) > 0;
            if (conditionMatches && !alreadyTriggered) 
            {
                item.ToTrigger?.Invoke(m_state.Current);
            }
        }
    }
}
