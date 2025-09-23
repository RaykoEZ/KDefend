using System.Collections.Generic;
using UnityEngine;
// changes shop item pool when player obtains/triggers a game event
public class ShopPoolUpdater : MonoBehaviour
{
    // collection containing all obtainable items, all index accesses this collection
    [SerializeField] ItemAssetLookup m_mainCollection = default;
    [SerializeField] ShopPoolEventList m_eventList = default;
    // starting item list
    [SerializeField] List<int> m_defaultIndexList = default;
    protected List<UpdateShopPool> m_events;
    HashSet<int> m_currentUnlockedSet;
    // the pool to add
    protected HashSet<int> m_addedPool = new HashSet<int>();
    public IReadOnlyCollection<int> AddedPool => m_addedPool;
    void OnEnable()
    {
        Init();
    }
    void OnDisable()
    {
        Shutdown();
    }
    public void UpdatePool()
    {
        // set to default, add bonus pools from updater
        m_currentUnlockedSet = new HashSet<int>(m_defaultIndexList);
        m_currentUnlockedSet.UnionWith(AddedPool);
    }
    public List<ItemAsset> GetOptionAssetPool()
    {
        List<ItemAsset> ret = new List<ItemAsset>();
        ItemAsset asset;
        if (m_currentUnlockedSet == null) return ret;
        // access items pool to get item assets
        foreach (var itemIndex in m_currentUnlockedSet) 
        {
            asset = m_mainCollection?.GetItemAssetByIndex(itemIndex);
            if (asset == null) continue;
            ret.Add(asset);
        }
        return ret; 
    }
    public void Init()
    {
        m_events = m_eventList?.GetEvents();
        // Listen to events for unlocking items
        foreach (var item in m_events) 
        {
            item?.Init(this);
        }
        UpdatePool();
    }
    public void Shutdown() 
    {
        // Listen to events for specific effects
        foreach (var item in m_events)
        {
            item?.Shutdown();
        }
    }
    public void AddToPool(List<int> toAdd) 
    {
        m_addedPool.UnionWith(toAdd);
    }
    public void RemoveFromPool(List<int> toRemove) 
    {
        m_addedPool.ExceptWith(toRemove);
    }
}
