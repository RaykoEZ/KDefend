using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.Util;

// snapshot of the game state
[Serializable]
public struct KDefenderGameState 
{
    public int SecondsElapsed;
    public int EnemiesKilled;
    // State of player entities
    public EntityState CafeValue;
    public EntityState PlayerValue;
    public List<EntityState> HostileStates;
    public static KDefenderGameState Default =>
        new KDefenderGameState 
        {
            SecondsElapsed = 0,
            EnemiesKilled = 0,
            CafeValue = new EntityState { Hp = 5000, Position = new UnityEngine.Vector2(200f, 300f)},
            PlayerValue = new EntityState { Hp = 100, Position = UnityEngine.Vector2.zero}
        };
}
