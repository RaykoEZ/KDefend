using System.Collections.Generic;
using Curry.Events;
public delegate void KDGameEventTrigger(object sender, KDEventInfo args);
// handles game event triggers with code only
internal class InternalEventHandler
{
    // gloabl event callbacks
    static Dictionary<GameEventTriggerType, KDGameEventTrigger> s_globalEvents = new Dictionary<GameEventTriggerType, KDGameEventTrigger>();
    Dictionary<GameEventTriggerType, KDGameEventTrigger> m_localEvents = new Dictionary<GameEventTriggerType, KDGameEventTrigger>();
    #region Global Event Options
    public static void TriggerGlobalEvent(object sender, KDEventInfo args)
    {
        TriggerEvent(s_globalEvents, sender, args);
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
    public void TriggerLocalEvent(object sender, KDEventInfo args)
    {
        TriggerEvent(m_localEvents, sender, args);
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
        object sender, KDEventInfo args)
    {
        if (args == null) return;
        var type = args.TriggerType;
        if (!eventCollection.ContainsKey(type) || eventCollection[type] == null) return;
        eventCollection[type]?.Invoke(sender, args);
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