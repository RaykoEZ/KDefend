using System.Collections.Generic;

public struct GameEventContext 
{
    public KDefenderGameState State;
    public readonly Dictionary<GameEventTriggerType, object> EventPayload;
    public GameEventContext(KDefenderGameState state, 
        Dictionary<GameEventTriggerType, object> payload) 
    {
        State = state;
        EventPayload = payload;
    }
}
