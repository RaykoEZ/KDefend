using Curry.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ShopItemState
{
    public int ItemIdx;
    public int Price;
    public int BuyLimit;
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
        base(copy.m_triggerConditions, copy.m_raiseOnTrigger, copy.m_triggerType, copy.m_triggerTypeOnRaise)
    {
        m_itemIndexList = copy.m_itemIndexList;
        m_updaterRef = copy.m_updaterRef;
    }
    public void Init(ShopPoolUpdater updater) 
    {
        m_updaterRef = updater;
        InternalEventHandler.ListenToGlobal(m_triggerType, ToInvoke);
    }
    public void Shutdown() 
    {
        m_updaterRef = null;
        InternalEventHandler.UnlistenFromGlobal(m_triggerType, ToInvoke);
    }
    protected override void Trigger_Internal(object sender, KDEventInfo args)
    {
        m_updaterRef?.UpdatePool(m_itemIndexList);
    }
}