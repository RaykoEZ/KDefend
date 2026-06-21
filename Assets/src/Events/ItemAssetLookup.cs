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
        return m_items.Find((i) => i.PrefabRef.Property.Equals(prop));
    }
    public ItemAsset GetItemAssetByIndex(string id) 
    {
        if (id == null) return null;

        return m_items.Find((i) => i.PrefabRef.Property.Id == id);
    }
}
[Serializable]
public class ItemAsset : IEquatable<ItemAsset>
{
    [SerializeField] Item m_asset = default;
    [SerializeField] Sprite m_cardArt = default;
    [SerializeField] Sprite m_cardBack = default;
    [NonSerialized] ShopItemState m_shopState = default;
    public Item PrefabRef => m_asset;
    public Sprite CardArt => m_cardArt;
    public Sprite CardBack => m_cardBack;
    public ShopItemState ShopState { get => m_shopState; set => m_shopState = value; }
    public bool Equals(ItemAsset other)
    {
        return (other.PrefabRef == null && PrefabRef == null) ||
            other.PrefabRef.Property.Name == PrefabRef.Property.Name &&
            other.PrefabRef.Property.Description == PrefabRef.Property.Description;
    }
    public override bool Equals(object obj)
    {
        return Equals(obj as ItemAsset);
    }
    public override int GetHashCode()
    {
        return PrefabRef == null? -1 : ($"{PrefabRef.Property.Name}").GetHashCode();
    }
}