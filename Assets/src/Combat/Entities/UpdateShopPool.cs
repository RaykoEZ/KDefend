using Curry.Events;
using System;
using UnityEngine;
public enum ShopPoolOperation
{
    Add,
    Remove,
}
// event for updating shop item pool, e.g. add/remove items from drop pool
[Serializable]
public class UpdateShopPool : KDEvent
{
    [SerializeField] ShopPoolOperation m_operationType = default;
    [SerializeField] ItemAssetLookup m_itemPool = default;
    protected ShopPoolUpdater m_updaterRef;
    // ctors
    public UpdateShopPool(KD_StaticEventFlags triggerConditions, KD_StaticEventFlags raiseOnTigger,
        ShopPoolOperation operationType, GameEventTriggerType triggerType, GameEventTriggerType triggerTypeToRaise,
        ItemAssetLookup itemPool, ShopPoolUpdater updaterRef) : 
        base(triggerConditions, raiseOnTigger, triggerType, triggerTypeToRaise)
    {
        m_operationType = operationType;
        m_itemPool = itemPool;
        m_updaterRef = updaterRef;
    }
    public UpdateShopPool(UpdateShopPool copy) :
        base(copy.m_triggerConditions, copy.m_raiseOnTrigger, copy.m_triggerType, copy.m_triggerTypeOnRaise)
    {
        m_operationType = copy.m_operationType;
        m_itemPool = copy.m_itemPool;
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
        switch (m_operationType)
        {
            case ShopPoolOperation.Add:
                m_updaterRef?.AddToPool(m_itemPool.Assets);
                break;
            case ShopPoolOperation.Remove:
                m_updaterRef?.RemoveFromPool(m_itemPool.Assets);
                break;
            default:
                // add to pool by default, might change this
                m_updaterRef?.AddToPool(m_itemPool.Assets);
                break;
        }
    }
}