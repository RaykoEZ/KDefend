using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemDrop_", menuName = "New Item Drops", order = 0)]
public class ItemDropList : ScriptableObject 
{
    [SerializeField] List<ItemDrop_Internal> m_dropList = default;

    [Serializable]
    protected struct ItemDrop_Internal : IWeightedItem
    {
        [SerializeField] Item m_toDrop;
        [Range(0, 100)]
        [SerializeField] int m_dropWeight;
        public int Weight => m_dropWeight;
        public Item ToDrop => m_toDrop;
        public static List<Item> ToItems(List<ItemDrop_Internal> list) 
        { 
            List<Item> ret = new List<Item>();
            foreach (var item in list)
            {
                ret.Add(item.ToDrop);
            }
            return ret;
        }
    }
    // get all item assets in collection
    public List<Item> GetDropListItemAsset()
    {
        return ItemDrop_Internal.ToItems(m_dropList);
    }
    public List<Item> GetWeightedDrops(int numToGet = 1, bool uniqueDrops = true) 
    {
        List<Item> ret = new List<Item>(numToGet);
        if (numToGet <= 0) return ret;
        List<ItemDrop_Internal> result = SamplingUtil.SampleWithWeights(m_dropList, numToGet, uniqueDrops);
        return ItemDrop_Internal.ToItems(result);
    }
    public Item GetWeightedDrop() 
    {
        return SamplingUtil.SampleWithWeights(m_dropList).ToDrop;
    }
}
