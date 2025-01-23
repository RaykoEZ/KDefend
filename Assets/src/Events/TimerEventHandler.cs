using System.Collections.Generic;
// handle any timer events
public class TimerEventHandler<T>
{
    public List<ITimerEvent<T>> Events => m_events;
    protected List<ITimerEvent<T>> m_events;
    // Get a copy of the current game state from a state manager
    T m_currentState;
    Dictionary<int, List<ITimerEvent<T>>> m_eventSet;
    public TimerEventHandler() 
    {
        m_events = new List<ITimerEvent<T>>();
        m_eventSet = new Dictionary<int, List<ITimerEvent<T>>>();
    }
    public TimerEventHandler(List<ITimerEvent<T>> events)
    {
        m_events = new List<ITimerEvent<T>>(events);
        m_eventSet = new Dictionary<int, List<ITimerEvent<T>>>();
        AddEvents(m_events);
    }
    // add new events with this
    public virtual void AddEvents(List<ITimerEvent<T>> newEvents) 
    {
        int time;
        foreach (var item in newEvents)
        {
            time = item.TriggerTime;
            // create new time event collection for a time frame
            // if no other events trigger in this time frame 
            if (!m_eventSet.ContainsKey(time)) 
            {
                m_eventSet.Add(time, new List<ITimerEvent<T>>());
            }
            // add the invoke event here
            m_eventSet[time]?.Add(item);
        }
    }
    // Listen to timer with this
    public void OnTimeElapsed(int secondsElapsed) 
    {
        if (m_eventSet.TryGetValue(secondsElapsed, out var events))
        {
            var removeList = new List<ITimerEvent<T>>();
            foreach (var item in events)
            {
                item?.ToInvoke?.Invoke(m_currentState);
                if (item.TriggersOnce) 
                {
                    removeList.Add(item);
                }
            }
            // cleanup one-time events
            foreach (var item in removeList)
            {
                events.Remove(item);
            }
        }
    }
}
