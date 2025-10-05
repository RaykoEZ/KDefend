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
        int idx;
        for (int i = 0; i < toUpdate.Count; i++) 
        {
            // try to find existing shop item entry
            // add new item into the shop pool if no duplicate indexed field
            // if add failed, apply changes to existing data field
            idx = toUpdate[i].ItemIdx;
            var findResult = m_currentShopPool.Find((x) => x.ItemIdx == idx);
            if (findResult != null)
            {
                // add price and buy limit
                // negative allowed for decreasing price/amount left
                findResult.Price = Mathf.Max(0, findResult.Price + toUpdate[i].Price);
                findResult.BuyLimit = Mathf.Max(0, findResult.BuyLimit + toUpdate[i].BuyLimit);
            }
            else 
            {
                m_currentShopPool.Add(toUpdate[i]);
            }
        }
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