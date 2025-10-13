using Curry.Events;
using System;
using System.Collections.Generic;
using System.IO;

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
    // trigger special pause when using full access stop watch
    public static void Pause_0x(object sender, bool isStopped, bool fullAccess, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        Dictionary<string, object> args = new Dictionary<string, object> { { "isOn", isStopped }, { "fullAccess", fullAccess }};
        InternalEventHandler.TriggerGlobalEvent(sender,
            new KDEventInfo(raiseFlag, GameEventTriggerType.FullAccess_tool0x, args));
    }
    // Conitinuous Credit Drain switch
    public static void CreditChangeOverTime(object sender, bool isOn, float timeInterval, int creditDeltaPerTick, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None) 
    {
        Dictionary<string, object> args = new Dictionary<string, object> { { "timeInterval", timeInterval }, { "isOn", isOn }, {"delta", creditDeltaPerTick } };
        InternalEventHandler.TriggerGlobalEvent(sender,
            new KDEventInfo(raiseFlag, GameEventTriggerType.CreditOverTime, args));
    }
    // Rewind time and days with tool13x to unlock Monday
    // On full access, trigger Special Ending - Goodbye World
    public static void HelloWorld_13x(object sender, bool fullAccess, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        Dictionary<string, object> args = new Dictionary<string, object> { { "fullAccess", fullAccess } };
        InternalEventHandler.TriggerGlobalEvent(sender,
            new KDEventInfo(raiseFlag, GameEventTriggerType.Access_Tool13x_Rewind, args));
    }
    // Drop an item from a weighted list of item drops
    public static void ItemDropEvent(object sender, ItemDropList dropList, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        List<ItemAsset> options = dropList?.GetWeightedDrops(3);
        KDEventInfo args = new KDEventInfo(raiseFlag,
            GameEventTriggerType.ItemOption, new Dictionary<string, object> { { "options", options } });
        InternalEventHandler.TriggerGlobalEvent(sender, args);
    }
    // When player obtains items outside of pickup from drops
    public static void ObtainItemEvent(object sender, ItemAsset toObtain, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None) 
    {
        // update player inventory, setup item effects & trigger events for obtaining the item
        KDEventInfo args = new KDEventInfo(raiseFlag, GameEventTriggerType.ItemObtained,
            new Dictionary<string, object> { { "item", toObtain } });
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
