using Curry.Events;
using System;
using System.Collections.Generic;
// snapshot of the game state
[Serializable]
public struct KDefenderGameState
{
    public int Timer;
    public KD_StaticEventFlags StaticFlags;
    // State of player entities
    public EntityState PlayerValue;
    public List<EnemyState> HostileStates;
    public List<ItemState> HeldItems;
    //public List<string> ActiveDeliveries;
    public List<ShopItemState> ShopStates;
    public KDefenderGameState(KDefenderGameState copy) 
    {
        Timer = copy.Timer;
        StaticFlags = copy.StaticFlags;
        PlayerValue = copy.PlayerValue;
        HostileStates = copy.HostileStates;
        HeldItems = copy.HeldItems;
        //ActiveDeliveries = copy.ActiveDeliveries;
        ShopStates = copy.ShopStates;
    }
}
[Serializable]
public struct InventoryState 
{
    public List<ItemProperty> ItemProperties;
}