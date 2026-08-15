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
    // use for game event trigger with specific flags
    public class KDEventInfo : EventInfo
    { 
        public KD_StaticEventFlags Flags { get; protected set; }

        public GameEventTriggerType TriggerType { get; protected set; }

        public KDEventInfo(KD_StaticEventFlags flags, GameEventTriggerType triggerType, Dictionary<string, object> payload = null, Action onFinishCallback = null)
        {
            Payload = payload;
            OnFinishedCallback = onFinishCallback;
            Flags = flags;
            TriggerType = triggerType;
        }
    }
}

