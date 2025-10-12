using System;

[Flags]
public enum KD_StaticEventFlags 
{
    None = 0,
    NewGame = 1 << 0,
    Tut_Control_Movement = 1 << 1,
    Tut_Deli_Intel = 1 << 2,
    Shop_Visited = 1 << 3,  
    // player defeated snipe boss
    Sniper_Defeat = 1 << 4,
    // player used R.I Letter to mediate conflict vs sniper
    Sniper_Mediated = 1<< 5,
    // Player extracts & clones the power of Dev0 from pause menu.
    // Dev0 is dropped into level and collected by the Shopkeeper.
    Pause_Clone = 1<< 6,
    // Purchased Dev0_Pause (fragment) from shop
    // Make watch appear on screen
    // Allow waych to be dragged out of game window
    Dev0_Obtained = 1 << 7,
    // flag for player dragging Dev0 out of game window
    // output clue files to player desktop
    Dev0_Extract = 1 << 8,
    // Raise after player drag in dev0's full access version
    Dev0_FullAccess = 1 << 9,
    // Unlocks Monday & devtool 13 (part 1) in shop
    // 1. defeat Tuesday/Friday boss
    Boss_Defeated = 1 << 10,
    // Player opens room mechanism in hidden bridge path
    // 1.Door Input given by boss drop clues on Tues/Fri 
    // 2.Stop Monday Boss (Does not have Dev_13_Rewind)
    // 3.Player need to attempt on Monday, input device broken on other days
    SecretRoom_Opened = 1 << 11,
}
// what kind of event is expected
[Serializable]
public enum GameEventTriggerType
{
    None,
    TimeElapsed,
    CreditOverTime,
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
    OnFriday,
    OnDayStart,
    OnDayEnd,
    // Using dev tools
    FullAccess_tool0x,
    Access_Tool13x_Rewind,
}