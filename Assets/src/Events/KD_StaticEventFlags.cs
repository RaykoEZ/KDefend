using System;

[Flags]
public enum KD_StaticEventFlags 
{
    None = 0,
    NewGame = 1 << 0,
    Tut_Control_Movement = 1 << 1,
    // player defeated snipe boss
    Sniper_Defeat = 1 << 4,
    // player used R.I Letter to mediate conflict vs sniper
    Sniper_Mediated = 1<< 5,
    // Purchased Dev0_Pause (fragment) from shop
    // Make watch appear on screen
    // Allow waych to be dragged out of game window
    Dev0_Obtained = 1 << 7,
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
    TimeUpdate,
    CreditUpdate,
    CreditOverTime,
    ItemObtained,
    ItemDragIn,
    ItemDragOut,
    ItemOption,
    Spawn,
    EnemyDefeated,
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