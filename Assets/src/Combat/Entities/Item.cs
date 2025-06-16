using System;
using System.Collections.Generic;
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
public interface IItem<T>
{
    ItemProperty Property { get; }
    // Use as counter from item amount/level
    int StackCount { get; set; }
    public void UseItem(T user);
    public void OnPickup(T user);
}
[RequireComponent(typeof(Collider2D))]
// class to contain item property and interaction triggers
public class Item : MonoBehaviour , IItem<Player>
{
    [SerializeField] protected ItemProperty m_property = default;
    [SerializeField] protected UnityEvent<Player> m_onUse = default;
    [SerializeField] protected UnityEvent<Player> m_onPickup = default;
    [SerializeField] List<EffectModule> m_effects = default;
    protected int m_stackCount = 1;
    protected bool m_isEffectActive = false;
    protected Player m_user;
    public ItemProperty Property => m_property;
    public int StackCount { get => m_stackCount; set => m_stackCount = value; }
    // Pickup trigger
    void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.attachedRigidbody == null) return;
        // when projectile hit this body, trigger on hit effects from projectile
        if (col.attachedRigidbody.TryGetComponent(out Player result))
        {
            m_user = result;
            OnPickup(result);
        }
    }  
    public virtual void OnPickup(Player player)
    {
        m_onPickup?.Invoke(player);
    }
    public virtual void UseItem(Player player)
    {
        m_isEffectActive = true;
        m_onUse?.Invoke(player);
    }
}