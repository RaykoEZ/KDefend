using System.Collections.Generic;

public struct KDefenderEventContext 
{
    public KDefenderGameState State;
    public readonly Dictionary<GameEventTriggerType, object> EventPayload;
    public KDefenderEventContext(KDefenderGameState state, 
        Dictionary<GameEventTriggerType, object> payload) 
    {
        State = state;
        EventPayload = payload;
    }
}
