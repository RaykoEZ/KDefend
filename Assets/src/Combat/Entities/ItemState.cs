using System;
using UnityEngine;

[Serializable]
public class ItemState 
{
    public ItemProperty Property = default;
    public int StackCount = default;
    public ItemState(ItemProperty property, int stacks) 
    {
        Property = property;
        StackCount = stacks;
    }
}
[Serializable]
public struct ItemProperty : IEquatable<ItemProperty>
{
    public string Id;
    public string Name;
    [TextArea(minLines: 1, maxLines: 2)]
    public string Description;
    public bool Equals(ItemProperty other)
    {
        return other.Name == Name && other.Description == Description;
    }
    public override int GetHashCode()
    {
        return ($"{Name}/{Description}").GetHashCode();
    }
}