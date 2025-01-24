using System.Collections.Generic;
using UnityEngine;
// A list of timer events to trigger in a list, for a level/wave
public class TimerEventManager : MonoBehaviour 
{
    [SerializeField] List<KDefenderTimerEvent> m_eventsToTrigger = default;
    TimerEventHandler<KDefenderTimerEvent, KDefenderGameState> m_timerEvents = 
        new TimerEventHandler<KDefenderTimerEvent, KDefenderGameState>();
    public List<KDefenderTimerEvent> EventsToTrigger { get => m_eventsToTrigger; }
    void Start()
    {
        m_timerEvents.AddEvents(m_eventsToTrigger);
    }
    public void OnTimeElapsed(int secondsElapsed) 
    {
        m_timerEvents?.OnTimeElapsed(secondsElapsed);
    }
}
