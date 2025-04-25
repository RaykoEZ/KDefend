using System;
using System.Reflection;

namespace Curry.Events
{
    [Serializable]
    public class CurryGameEventTrigger
    {
        public CurryGameEventSource m_eventToTrigger = default;
        public bool TryTrigger() 
        {
            if (m_eventToTrigger == null) return false;
            return m_eventToTrigger.TryBroadcast(new EventInfo());
        }
        public bool TryTrigger(EventInfo info) 
        {
            if (m_eventToTrigger == null) return false;
            return m_eventToTrigger.TryBroadcast(info);
        }
        public void TriggerEvent(EventInfo eventInfo)
        {
            m_eventToTrigger?.Broadcast(eventInfo);
        }
        public void TriggerEvent()
        {
            m_eventToTrigger?.Broadcast(new EventInfo());
        }
    }
}