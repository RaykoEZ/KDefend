using Curry.Events;
using System;
using System.Collections.Generic;
using System.IO;

internal static class KDEventUtil 
{
    #region Global Game Event Calls
    // trigger to set game as paused/ not paused
    public static void PauseGame(object sender, bool gameActive, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        Dictionary<string, object> args = new Dictionary<string, object> { { "isOn", gameActive } };
        KDEventHandler.TriggerGlobalEvent(sender,
            new KDEventInfo(raiseFlag, GameEventTriggerType.PauseGame, args));
    }
    // pause game timer
    public static void PauseTimer(object sender, bool gameActive, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        Dictionary<string, object> args = new Dictionary<string, object> { { "isOn", gameActive } };
        KDEventHandler.TriggerGlobalEvent(sender,
            new KDEventInfo(raiseFlag, GameEventTriggerType.PauseTime, args));
    }
    // trigger special pause when using full access stop watch
    public static void Pause_0x(object sender, bool gameActive, bool fullAccess, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        Dictionary<string, object> args = new Dictionary<string, object> { { "isOn", gameActive }, { "fullAccess", fullAccess } };
        KDEventHandler.TriggerGlobalEvent(sender,
            new KDEventInfo(raiseFlag, GameEventTriggerType.FullAccess_tool0x, args));
    }
    // add time
    public static void ExtendTimer(object sender, int extendTime, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        Dictionary<string, object> args = new Dictionary<string, object> { { "extend", extendTime } };
        KDEventHandler.TriggerGlobalEvent(sender,
            new KDEventInfo(raiseFlag, GameEventTriggerType.TimeUpdate, args));
    }
    // When player's credit reach the level target, trigger event
    public static void CreditTargetReached(object sender, int creditOwned, int targetReached, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None) 
    {
        Dictionary<string, object> args = new Dictionary<string, object> { { "owned", creditOwned }, { "reached", targetReached } };
        KDEventHandler.TriggerGlobalEvent(sender,
            new KDEventInfo(raiseFlag, GameEventTriggerType.CreditUpdate, args));
    }

    // Conitinuous Credit Drain switch
    public static void CreditChangeOverTime(object sender, bool isOn, float timeInterval, int creditDeltaPerTick, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None) 
    {
        Dictionary<string, object> args = new Dictionary<string, object> { { "timeInterval", timeInterval }, { "isOn", isOn }, {"delta", creditDeltaPerTick } };
        KDEventHandler.TriggerGlobalEvent(sender,
            new KDEventInfo(raiseFlag, GameEventTriggerType.CreditOverTime, args));
    }
    // Rewind time and days with tool13x to unlock Monday
    // On full access, trigger Special Ending - Goodbye World
    public static void HelloWorld_13x(object sender, bool fullAccess, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        Dictionary<string, object> args = new Dictionary<string, object> { { "fullAccess", fullAccess } };
        KDEventHandler.TriggerGlobalEvent(sender,
            new KDEventInfo(raiseFlag, GameEventTriggerType.Access_Tool13x_Rewind, args));
    }
    // Drop an item from a weighted list of item drops
    public static void ItemDropEvent(object sender, ItemDropList dropList, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        List<ItemAsset> options = dropList?.GetWeightedDrops(3);
        KDEventInfo args = new KDEventInfo(raiseFlag,
            GameEventTriggerType.ItemOption, new Dictionary<string, object> { { "options", options } });
        KDEventHandler.TriggerGlobalEvent(sender, args);
    }
    // When player obtains items outside of pickup from drops
    public static void ObtainItemEvent(object sender, ItemAsset toObtain, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None) 
    {
        // update player inventory, setup item effects & trigger events for obtaining the item
        KDEventInfo args = new KDEventInfo(raiseFlag, GameEventTriggerType.ItemObtained,
            new Dictionary<string, object> { { "item", toObtain } });
        KDEventHandler.TriggerGlobalEvent(sender, args);

    }
    // Spawn a wave of enemies
    public static void SpawnEnemies(object sender, SpawnWave wave, KD_StaticEventFlags raiseFlag = KD_StaticEventFlags.None)
    {
        KDEventInfo args = new KDEventInfo(raiseFlag,
    GameEventTriggerType.Spawn, new Dictionary<string, object> { { "wave", wave } });
        KDEventHandler.TriggerGlobalEvent(sender, args);
    }
    #endregion
}
