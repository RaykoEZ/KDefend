using Curry.Events;
using System.Collections.Generic;

internal static class KDEventUtil 
{
    #region Global Game Event Calls
    // trigger to set game as paused/ not paused
    public static void PauseGame(object sender, bool isStopped, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        Dictionary<string, object> args = new Dictionary<string, object> { { "isOn", isStopped } };
        InternalEventHandler.TriggerGlobalEvent(sender,
                new KDEventInfo(raiseFlag, GameEventTriggerType.PauseGame, args));
    }
    // pause game timer
    public static void PauseTimer(object sender, bool isStopped, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None) 
    {
        Dictionary<string, object> args = new Dictionary<string, object> { { "isOn", isStopped } };
        InternalEventHandler.TriggerGlobalEvent(sender,
        new KDEventInfo(raiseFlag, GameEventTriggerType.PauseTime, args));
    }
    // Drop an item from a weighted list of item drops
    public static void ItemDropEvent(object sender, ItemDropList dropList, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        List<ItemAsset> options = dropList?.GetWeightedDrops(3);
        KDEventInfo args = new KDEventInfo(raiseFlag,
            GameEventTriggerType.ItemOption, new Dictionary<string, object> { { "options", options } });
        InternalEventHandler.TriggerGlobalEvent(sender, args);
    }
    // Spawn a wave of enemies
    public static void SpawnEnemies(object sender, SpawnWave wave, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        KDEventInfo args = new KDEventInfo(raiseFlag,
    GameEventTriggerType.Spawn, new Dictionary<string, object> { { "wave", wave } });
        InternalEventHandler.TriggerGlobalEvent(sender, args);
    }
    #endregion
}
