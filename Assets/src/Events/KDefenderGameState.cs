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
}
