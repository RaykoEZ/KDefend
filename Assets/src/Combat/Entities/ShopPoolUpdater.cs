using System.Collections.Generic;
using UnityEngine;
// changes shop item pool when player obtains/triggers a game event
public class ShopPoolUpdater : MonoBehaviour
{
    [SerializeField] ShopPoolEventList m_eventList = default;
    protected List<UpdateShopPool> m_events;
    // the pool to add
    protected HashSet<ItemAsset> m_addedPool = new HashSet<ItemAsset>();
    public IReadOnlyCollection<ItemAsset> AddedPool => m_addedPool;
    void OnEnable()
    {
        Init();
    }
    void OnDisable()
    {
        Shutdown();
    }
    public void Init()
    {
        m_events = m_eventList?.GetEvents();
        // Listen to events for specific effects
        foreach (var item in m_events) 
        {
            item?.Init(this);
        }
    }
    public void Shutdown() 
    {
        // Listen to events for specific effects
        foreach (var item in m_events)
        {
            item?.Shutdown();
        }
    }
    public void AddToPool(List<ItemAsset> toAdd) 
    {
        m_addedPool.UnionWith(toAdd);
    }
    public void RemoveFromPool(List<ItemAsset> toRemove) 
    {
        m_addedPool.ExceptWith(toRemove);
    }
}
