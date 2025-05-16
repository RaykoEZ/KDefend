using System;

[Flags]
public enum KD_StaticEventFlags 
{
    None = 0,
    Tut_Control_Movement = 1 << 0,
    Tut_Deli_Food = 1 << 1,
    Tut_Deli_Intel = 1 << 2,
    Tut_Deli_Contraband = 1 << 3,
    Shop_Visited = 1 << 4,
    Watch_Obtained = 1 << 5,
    NewGame = 1 << 6,
}