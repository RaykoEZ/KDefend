using System.Collections.Generic;
// handle any timer events
public class TimerEventHandler<T0> where T0 : ITimerEvent<KDefenderEventContext>
{
    public List<T0> Events => m_events;
    protected List<T0> m_events;
    // Get a copy of the current game state from a state manager
    KDefenderEventContext m_currentState;
    Dictionary<int, List<T0>> m_eventSet;
    public TimerEventHandler() 
    {
        m_events = new List<T0>();
        m_eventSet = new Dictionary<int, List<T0>>();
    }
    public TimerEventHandler(List<T0> events)
    {
        m_events = new List<T0>(events);
        m_eventSet = new Dictionary<int, List<T0>>();
        AddEvents(m_events);
    }
    // add new events with this
    public virtual void AddEvents(List<T0> newEvents) 
    {
        int time;
        foreach (var item in newEvents)
        {
            time = item.TriggerTime;
            // create new time event collection for a time frame
            // if no other events trigger in this time frame 
            if (!m_eventSet.ContainsKey(time)) 
            {
                m_eventSet.Add(time, new List<T0>());
            }
            // add the invoke event here
            m_eventSet[time]?.Add(item);
        }
    }
    // Listen to timer with this
    public void OnTimeElapsed(KDefenderEventContext e) 
    {
        if (e.EventPayload == null || !e.EventPayload.ContainsKey(GameEventTriggerType.Time)) return;
        int secondsElapsed = (int)e.EventPayload[GameEventTriggerType.Time];
        if (m_eventSet.TryGetValue(secondsElapsed, out var events))
        {
            var removeList = new List<T0>();
            foreach (var item in events)
            {
                item?.ToInvoke?.Invoke(m_currentState);
                removeList.Add(item);
            }
            // cleanup one-time events
            foreach (var item in removeList)
            {
                events.Remove(item);
            }
        }
    }
}