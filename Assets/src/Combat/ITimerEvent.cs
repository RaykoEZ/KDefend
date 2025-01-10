using UnityEngine.Events;

public interface ITimerEvent<T> 
{
    bool TriggersOnce { get; }
    int TriggerTime { get; }
    UnityEvent<T> ToInvoke { get; }
}
