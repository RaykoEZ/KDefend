using System;
using System.Collections.Generic;

namespace Curry.Events 
{
    [Serializable]
    public class EventInfo 
    {
        public Dictionary<string, object> Payload { get; protected set; }
        // Method to call after resolving this event
        public Action OnFinishedCallback { get; protected set; }
        public EventInfo(Dictionary<string, object> payload = null, Action onFinishCallback = null) 
        {
            Payload = payload;
            OnFinishedCallback = onFinishCallback;
        }
    }
    public interface IKDEvent
    {
        public GameEventTriggerType TriggerType { get; }
        public KD_StaticEventFlags Flags { get; }

    }
    // use for game event trigger with specific flags
    public class KDNpcEventInfo : EventInfo , IKDEvent
    { 
        public KD_StaticEventFlags Flags { get; protected set; }

        public GameEventTriggerType TriggerType { get; protected set; }

        public KDNpcEventInfo(KD_StaticEventFlags flags, GameEventTriggerType triggerType, Dictionary<string, object> payload = null, Action onFinishCallback = null)
        {
            Payload = payload;
            OnFinishedCallback = onFinishCallback;
            Flags = flags;
            TriggerType = triggerType;
        }
    }
}

