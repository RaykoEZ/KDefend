using System;

[Flags]
public enum KD_StaticEventFlags 
{
    None = 0,
    Tut_Control_Movement = 1 << 0,
    Tut_Deli_Intel = 1 << 1,
    Shop_Visited = 1 << 2,  
    // player defeated snipe boss
    Sniper_Defeat,
    // player used R.I Letter to mediate conflict vs sniper
    Sniper_Mediated,
    // Purchased Dev0_Pause (fragment) from shop
    // Make watch appear on screen
    // Allow waych to be dragged out of game window
    Watch_Obtained = 1 << 3,
    NewGame = 1 << 4,
    // flag for player dragging Dev0 out of game window
    // output clue files to player desktop
    Dev0_Extract = 1 << 5,
    // Raise after player drag in dev0's full access version
    Dev0_FullAccess = 1 << 6,
    // Unlocks devtool 13 (part 1) in shop
    // 1. defeat Tuesday/Friday boss
    // 2. Purchase Dev13 from shop and use it to trigger to go to Monday
    Dev13_UnlockMonday = 1 << 7,
    // Player opens room mechanism in hidden bridge path
    // 1.Door Input given by boss drop on Tues/Fri 
    // 2.Stop Monday Boss (Does not have Dev_13_Rewind)
    // 3.Player need to attempt on Monday, input device broken on other days
    SecretRoom_Opened = 1 << 8
}
// what kind of event is expected
[Serializable]
public enum GameEventTriggerType
{
    None,
    TimeElapsed,
    ItemObtained,
    ItemOption,
    Spawn,
    EnemyDefeated,
    AreaReached,
    OnAttackBegin,
    OnAttackFinish,
    OnTakeDamage,
    OnDefeat,
    PauseGame,
    PauseTime,
    // triggers every time a new day begins
    OnMonday,
    OnTuesday,
    OnWednesday,
    OnThursday,
    OnFriday
}