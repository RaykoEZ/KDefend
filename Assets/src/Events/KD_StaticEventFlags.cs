using System;

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
}