using System;
using System.Collections.Generic;
using UnityEngine;
// a list of item assets, access them with index
[CreateAssetMenu(fileName = "ItemLookup_", menuName = "Jams/Item/New Lookup list")]
public class ItemAssetLookup : ScriptableObject
{
    [SerializeField] List<ItemAsset> m_items = default;
    
    public List<ItemAsset> GetUniqueRandomItems(int numToGet = 3) 
    {
        List<ItemAsset> ret = new List<ItemAsset>();
        if (numToGet < 0 || m_items.Count == 0) return ret;
        // fit all into list if we don't have enough to get from list
        if (m_items.Count <= numToGet) 
        {
            ret.AddRange(m_items);
        }
        else 
        {
            // random uniqu results
            ret.AddRange(SamplingUtil.SampleFromList(m_items, numToGet, uniqueResults: true));
        }
        return ret;
    } 
    public int GetIdOf(ItemAsset item) 
    {
        if (item == null) return -1;
        return m_items.FindIndex((i) => i == item);
    }
    public ItemAsset GetItemAsset(int id) 
    {
        if (id < 0 || id >= m_items.Count) return null;
        return m_items[id];
    }
}
[Serializable]
public class ItemAsset 
{
    [SerializeField] Item m_asset;
    [SerializeField] Sprite m_cardArt;
    [SerializeField] Sprite m_cardBack;
    public Item Asset => m_asset;
    public Sprite CardArt => m_cardArt;
    public Sprite CardBack => m_cardBack;
}