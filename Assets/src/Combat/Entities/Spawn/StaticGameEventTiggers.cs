using Curry.Events;
using System.Collections.Generic;
using UnityEngine;

public class StaticGameEventTiggers : MonoBehaviour 
{
    [SerializeField] List<CurryGameEventTrigger> m_triggers = default;
    public void TriggerEvent(int eventIndex) 
    {
        if (eventIndex > 0 && eventIndex < m_triggers.Count)
        {
            m_triggers[eventIndex]?.TriggerEvent();
        }
    }
}
