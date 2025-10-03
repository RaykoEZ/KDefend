using System.Collections.Generic;
using UnityEngine;
// changes shop item pool and their states (price & buyable units left)
// when player obtains/triggers a game event
public class ShopPoolUpdater : MonoBehaviour
{
    // collection containing all obtainable items, all index accesses this collection and its states
    [SerializeField] ShopPool m_defaultPool = default;
    [SerializeField] ShopPoolEventList m_eventList = default;
    protected List<ShopItemState> m_currentShopPool = new List<ShopItemState>();
    protected List<UpdateShopPool> m_events;
    public List<ShopItemState> CurrentShopPool => m_currentShopPool;
    void OnEnable()
    {
        InitEvents();
    }
    void OnDisable()
    {
        Shutdown();
    }
    void InitEvents()
    {
        m_events = m_eventList?.GetEvents();
        // Listen to events for unlocking items
        foreach (var item in m_events)
        {
            item?.Init(this);
        }
    }
    void Shutdown()
    {
        // Listen to events for specific effects
        foreach (var item in m_events)
        {
            item?.Shutdown();
        }
    }
    public void InitPool(List<ShopItemState> toSet)
    {
        bool isValid = toSet != null && toSet.Count > 0;
        // set to default, add bonus pools from updater
        m_currentShopPool = isValid? toSet :
            new List<ShopItemState> (m_defaultPool.DefaultPoolStates);
    }
    public void UpdatePool(List<ShopItemState> toUpdate)
    {
        if (toUpdate == null || toUpdate.Count == 0) return;
        for (int i = 0; i < toUpdate.Count; i++) 
        {
            if (i < m_currentShopPool.Count)
            {
                // add price and buy limit
                // negative allowed for decreasing price/amount left
                m_currentShopPool[i].Price = Mathf.Max(0, m_currentShopPool[i].Price + toUpdate[i].Price);
                m_currentShopPool[i].BuyLimit = Mathf.Max(0, m_currentShopPool[i].BuyLimit + toUpdate[i].BuyLimit);
            }
            else 
            {
                // new a list of new excess items and add them to the current state list
                List<ShopItemState> toAdd = new List<ShopItemState>(toUpdate);
                toAdd.RemoveRange(0, i);
                AddNewItemState(toAdd);
                return;
            }
        }
    }
    public void AddNewItemState(List<ShopItemState> toAdd) 
    {
        m_currentShopPool.AddRange(toAdd);
    }
    public List<ItemAsset> GetOptionAssetPool()
    {
        List<ItemAsset> ret = new List<ItemAsset>();
        ItemAsset asset;
        if (m_currentShopPool == null) return ret;
        // access items pool to get item assets
        for (int i = 0; i < m_currentShopPool.Count; i++)
        {
            asset = m_defaultPool?.ItemPoolLookup?.GetItemAssetByIndex(i);
            if (asset == null) continue;
            // setup shop state
            asset.ShopState = m_currentShopPool[i];
            ret.Add(asset);
        }
        return ret; 
    }
}