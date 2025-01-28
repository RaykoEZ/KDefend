using Curry.Events;
using System;
using System.Collections.Generic;

// snapshot of the game state
[Serializable]
public struct KDefenderGameState
{
    public int SecondsElapsed;
    public int EnemiesKilled;
    // State of player entities
    public EntityState CafeValue;
    public EntityState PlayerValue;
    public InventoryState Inventory;
    public List<EntityState> HostileStates;
    public List<GameObjective> Active;
    public List<GameObjective> Completed;
    public List<GameObjective> Failed;
}
[Serializable]
public struct InventoryState 
{
    public List<ItemProperty> ItemProperties;
    public List<int> ItemStackCount;
}