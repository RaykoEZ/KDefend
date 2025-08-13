using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
    public ItemAsset GetItemByProperty(ItemProperty prop) 
    {
        return m_items.Find((i) => i.Asset.Property.Equals(prop));
    }
    public ItemAsset GetItemAssetByIndex(int id) 
    {
        if (id < 0 || id >= m_items.Count) return null;
        return m_items[id];
    }
}
[Serializable]
public class ItemAsset : IEquatable<ItemAsset>
{
    [SerializeField] Item m_asset = default;
    [SerializeField] Sprite m_cardArt;
    [SerializeField] Sprite m_cardBack;
    public Item Asset => m_asset;
    public Sprite CardArt => m_cardArt;
    public Sprite CardBack => m_cardBack;
    public bool Equals(ItemAsset other)
    {
        return (other.Asset == null && Asset == null) ||
            other.Asset.Property.Name == Asset.Property.Name &&
            other.Asset.Property.Description == Asset.Property.Description;
    }
    public override bool Equals(object obj)
    {
        return Equals(obj as ItemAsset);
    }
    public override int GetHashCode()
    {
        return Asset == null? -1 : ($"{Asset.Property.Name}").GetHashCode();
    }
}