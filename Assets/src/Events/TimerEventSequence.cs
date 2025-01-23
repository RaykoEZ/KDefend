using System.Collections.Generic;
using UnityEngine;
// A list of timer events to trigger in a list, for a level/wave
[CreateAssetMenu(fileName = "TimeEvent_", menuName = "Events/Time Events", order = 1)]
public class TimerEventSequence : ScriptableObject 
{
    [SerializeField] List<KDefenderTimerEvent> m_eventsToTrigger = default;

    public List<KDefenderTimerEvent> EventsToTrigger { get => m_eventsToTrigger; }
}
