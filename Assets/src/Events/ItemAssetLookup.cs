using System;
using System.Collections.Generic;
using UnityEngine;
// a list of item assets, access them with index
[CreateAssetMenu(fileName = "ItemLookup_", menuName = "Jams/Item/New Lookup list")]
public class ItemAssetLookup : ScriptableObject
{
    [SerializeField] List<ItemAsset> m_items = default;   
    public List<ItemAsset> Assets => m_items;
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