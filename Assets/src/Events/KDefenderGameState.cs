using Curry.Events;
using System;
using System.Collections.Generic;

// snapshot of the game state
[Serializable]
public struct KDefenderGameState
{
    public int CurrentLevel;
    public int EnemiesKilled;
    public int CurrentRoutineWave;
    // State of player entities
    public EntityState PlayerValue;
    public InventoryState Inventory;
    public List<EnemyState> HostileStates;
    public List<DeliveryDetail> Completed;
    public List<DeliveryDetail> Active;
}
[Serializable]
public struct InventoryState 
{
    public List<ItemProperty> ItemProperties;
    public List<int> ItemStackCount;
}