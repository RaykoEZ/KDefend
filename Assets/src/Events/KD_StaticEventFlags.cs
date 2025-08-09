using System;
using System.Collections.Generic;

[Flags]
public enum KD_StaticEventFlags 
{
    None = 0,
    Tut_Control_Movement = 1 << 0,
    Tut_Deli_Intel = 1 << 1,
    Shop_Visited = 1 << 2,
    Watch_Obtained = 1 << 3,
    NewGame = 1 << 4,
}
// what kind of event is expected
[Serializable]
public enum GameEventTriggerType
{
    TimeElapsed,
    ItemObtained,
    EnemyDefeated,
    AreaReached,
    OnAttackBegin,
    OnAttackFinish,
    OnTakeDamage,
    OnDefeat,
    OnPlayerMovement,
}
public delegate void KDGameEventTrigger(GameEventTriggerType type, object sender);
// handles game event triggers with code only
public class InternalEventHandler
{
    // gloabl event callbacks
    static Dictionary<GameEventTriggerType, KDGameEventTrigger> s_globalEvents = new Dictionary<GameEventTriggerType, KDGameEventTrigger>();
    Dictionary<GameEventTriggerType, KDGameEventTrigger> m_localEvents = new Dictionary<GameEventTriggerType, KDGameEventTrigger>();
    #region Global Event Options
    public static void TriggerGlobalEvent(GameEventTriggerType type, object sender)
    {
        TriggerEvent(s_globalEvents, type, sender);
    }
    // listen to global event
    public static void ListenToGlobal(GameEventTriggerType eventType, KDGameEventTrigger toInvoke)
    {
        ListenTo(s_globalEvents, eventType, toInvoke);
    }
    // remove event callback from global list
    public static void UnlistenFromGlobal(
        GameEventTriggerType eventType, KDGameEventTrigger toUnlisten)
    {
        Unlisten(s_globalEvents, eventType, toUnlisten);
    }
    #endregion

    #region Local Event Options
    public void TriggerLocalEvent(GameEventTriggerType type, object sender)
    {
        TriggerEvent(m_localEvents, type, sender);
    }
    public void ListenToLocal(GameEventTriggerType eventType, KDGameEventTrigger toInvoke)
    {
        ListenTo(m_localEvents, eventType, toInvoke);
    }
    public void UnlistenFromLocal(GameEventTriggerType eventType, KDGameEventTrigger toUnlisten)
    {
        Unlisten(m_localEvents, eventType, toUnlisten);
    }
    #endregion

    #region Static Util
    static void TriggerEvent(Dictionary<GameEventTriggerType, KDGameEventTrigger> eventCollection,
        GameEventTriggerType type, object sender)
    {
        if (!eventCollection.ContainsKey(type) || eventCollection[type] == null) return;
        eventCollection[type]?.Invoke(type, sender);
    }
    static void Unlisten(Dictionary<GameEventTriggerType, KDGameEventTrigger> eventCollection,
        GameEventTriggerType eventType, KDGameEventTrigger toUnlisten)
    {
        if (!eventCollection.ContainsKey(eventType) || eventCollection[eventType] == null) return;
        eventCollection[eventType] -= toUnlisten;
    }
    static void ListenTo(Dictionary<GameEventTriggerType, KDGameEventTrigger> eventCollection,
        GameEventTriggerType type, KDGameEventTrigger toInvoke)
    {
        if (!eventCollection.ContainsKey(type))
        {
            eventCollection.Add(type, toInvoke);
        }
        else
        {
            eventCollection[type] += toInvoke;
        }
    }
    #endregion
}