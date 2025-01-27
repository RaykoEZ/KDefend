using System;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public struct ItemProperty
{
    public string Name;
    public string Description;
    public int ItemCost;
    public GameEventTriggerType TriggerType;
}
[Serializable]
public enum GameEventTriggerType
{
    Time,
    TakeDamage,
    Attack,
    ObtainItem,
    DiscardItem,
    StartOfMove,
    OnMoving
}
public interface IItem
{
    ItemProperty Property { get; }
    // Use as counter from item amount/level
    int StackCount { get; set; }
    public bool Activate(KDefenderEventContext e);
    public void OnPickup();
}
// class to contain item property and interaction triggers
public class Item : MonoBehaviour , IItem
{
    [SerializeField] protected ItemProperty m_property = default;
    [SerializeField] protected UnityEvent<Item> m_onUse = default;
    [SerializeField] protected UnityEvent<Item> m_onPickup = default;
    protected int m_stackCount = 1;
    public ItemProperty Property => m_property;
    public int StackCount { get => m_stackCount; set => m_stackCount = value; }
    public virtual void OnPickup()
    {
        m_onPickup?.Invoke(this);
    }
    public virtual bool Activate(KDefenderEventContext e)
    {
        m_onUse?.Invoke(this);
        return true;
    }
}