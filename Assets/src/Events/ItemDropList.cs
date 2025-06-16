using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemDrop_", menuName = "New Item Drops", order = 0)]
public class ItemDropList : ScriptableObject 
{
    [SerializeField] List<ItemDrop_Internal> m_dropList = default;
    [Serializable]
    public struct ItemDrop_Internal : IWeightedItem
    {
        [SerializeField] Item m_toDrop;
        [Range(0, 100)]
        [SerializeField] int m_dropWeight;
        public int Weight => m_dropWeight;
        public Item ToDrop => m_toDrop;
    }
    public List<Item> GetWeightedDrops(int numToGet = 1) 
    {
        List<Item> ret = new List<Item>(numToGet);
        if (numToGet <= 0) return ret;
        for (int i = 0; i < numToGet; i++)
        {
            ret.Add(SamplingUtil.SampleWithWeights(m_dropList).ToDrop);
        }
        return ret;
    }
    public Item GetWeightedDrop() 
    {
        return SamplingUtil.SampleWithWeights(m_dropList).ToDrop;
    }
}
