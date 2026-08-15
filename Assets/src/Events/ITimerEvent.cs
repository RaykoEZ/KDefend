using UnityEngine.Events;

public interface ITimerEvent<T> 
{
    int TriggerTime { get; }
    UnityEvent<T> ToInvoke { get; }
}
