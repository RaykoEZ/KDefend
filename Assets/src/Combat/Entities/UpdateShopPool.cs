using Curry.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
// We either add (remove via negative) or set item price and quanitity
public enum ShopPoolOperationType
{
    Add,
    Multiply,
    Set
}
[Serializable]
public class ShopItemState
{
    [SerializeField] ShopPoolOperationType m_shopOperation;
    public string ItemId;
    public int Price;
    public int BuyLimit;
    public ShopPoolOperationType ShopOperation => m_shopOperation;
}
// event for updating shop item pool, e.g. add/remove items from drop pool
[Serializable]
public class UpdateShopPool : KDEvent
{
    // Add or remove from shop pool?
    [SerializeField] List<ShopItemState> m_itemIndexList = default;
    protected ShopPoolUpdater m_updaterRef;
    // ctors
    public UpdateShopPool(KD_StaticEventFlags triggerConditions, KD_StaticEventFlags raiseOnTigger, GameEventTriggerType triggerType, GameEventTriggerType triggerTypeToRaise, 
        // shop pool ctor params
        List<ShopItemState> itemPool, ShopPoolUpdater updaterRef) : 
        base(triggerConditions, raiseOnTigger, triggerType, triggerTypeToRaise)
    {
        m_itemIndexList = itemPool;
        m_updaterRef = updaterRef;
    }
    public UpdateShopPool(UpdateShopPool copy) :
        base(copy.m_flagConditionsToTrigger, copy.m_raiseFlagOnTrigger, copy.m_triggerTypeToListen, copy.m_triggerTypeOnRaise)
    {
        m_itemIndexList = copy.m_itemIndexList;
        m_updaterRef = copy.m_updaterRef;
    }
    public void Init(ShopPoolUpdater updater) 
    {
        m_updaterRef = updater;
        KDEventHandler.ListenToGlobal(m_triggerTypeToListen, ToInvoke);
    }
    public void Shutdown() 
    {
        m_updaterRef = null;
        KDEventHandler.UnlistenFromGlobal(m_triggerTypeToListen, ToInvoke);
    }
    protected override void Trigger_Internal(object sender, KDEventInfo args)
    {
        m_updaterRef?.UpdatePool(m_itemIndexList);
    }
}