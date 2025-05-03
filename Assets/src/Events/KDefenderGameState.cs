using Curry.Events;
using System;
using System.Collections.Generic;

// snapshot of the game state
[Serializable]
public struct KDefenderGameState
{
    public int Timer;
    public int CurrentThreatLevel;
    // State of player entities
    public EntityState PlayerValue;
    public InventoryState Inventory;
    public List<EnemyState> HostileStates;
    public List<DeliveryDetail> Completed;
    public List<DeliveryDetail> Active;
    public KDefenderGameState(KDefenderGameState copy) 
    {
        Timer = copy.Timer;
        CurrentThreatLevel = copy.CurrentThreatLevel;
        PlayerValue = copy.PlayerValue;
        Inventory = copy.Inventory;
        HostileStates = copy.HostileStates;
        Completed = copy.Completed;
        Active = copy.Active;
    }
}
[Serializable]
public struct InventoryState 
{
    public List<ItemProperty> ItemProperties;
    public List<int> ItemStackCount;
}