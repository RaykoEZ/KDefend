using Curry.Events;
using System;
using System.Collections.Generic;
// snapshot of the game state
[Serializable]
public struct KDefenderGameState
{
    public int Timer;
    public KD_StaticEventFlags StaticFlags;
    public int CurrentThreatLevel;
    // State of player entities
    public EntityState PlayerValue;
    public InventoryState Inventory;
    public List<EnemyState> HostileStates;
    public List<string> CompletedDeliveries;
    public List<string> ActiveDeliveries;
    public KDefenderGameState(KDefenderGameState copy) 
    {
        Timer = copy.Timer;
        StaticFlags = copy.StaticFlags;
        CurrentThreatLevel = copy.CurrentThreatLevel;
        PlayerValue = copy.PlayerValue;
        Inventory = copy.Inventory;
        HostileStates = copy.HostileStates;
        CompletedDeliveries = copy.CompletedDeliveries;
        ActiveDeliveries = copy.ActiveDeliveries;
    }
}
[Serializable]
public struct InventoryState 
{
    public List<ItemProperty> ItemProperties;
    public List<int> ItemStackCount;
}