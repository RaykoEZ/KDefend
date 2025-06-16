using System;
using System.Collections.Generic;
public class Inventory<T> where T : IItem<Player>
{
    protected HashSet<T> m_itemSet;
    public List<T> ItemList => new List<T>(m_itemSet);
    public Inventory() 
    {
        m_itemSet = new HashSet<T>();
    }
    public Inventory(List<T> items) 
    {
        m_itemSet = new HashSet<T>();
        AddRange(items);
    }
    public List<T> Filter(Predicate<T> match)
    {
        return ItemList.FindAll(match);
    }
    public bool TryGetValue(T toGet, out T result) 
    {
        return m_itemSet.TryGetValue(toGet, out result);
    }
    public InventoryState GetState()
    {
        List<ItemProperty> prop = new List<ItemProperty>();
        List<int> stacks = new List<int>();
        foreach (var item in m_itemSet)
        {
            prop.Add(item.Property);
            stacks.Add(item.StackCount);
        }
        var ret = new InventoryState
        {
            ItemProperties = prop,
            ItemStackCount = stacks
        };
        return ret;
    }
    public void Remove(T toRemove) 
    {
        m_itemSet.Remove(toRemove);
    }
    public void Add(T toAdd) 
    {
        if (m_itemSet.TryGetValue(toAdd, out T result)) 
        {
            result.StackCount += toAdd.StackCount;
        }
        else 
        {
            m_itemSet.Add(toAdd);
        }
    }
    public void AddRange(List<T> toAdd)
    {
        foreach (T item in toAdd)
        {
            Add(item);
        }
    }
}
