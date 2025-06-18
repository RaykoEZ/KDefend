using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[Serializable]
public struct ItemProperty
{
    public string Name;
    public string Description;
    public int ItemCost;
}
[Serializable]
public enum GameEventTriggerType
{
    Time,
    Item,
    Attack,
    Movement
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
    [SerializeField] protected bool m_pickupImmediately = default;
    [SerializeField] protected ItemProperty m_property = default;
    [SerializeField] protected UnityEvent<Player> m_onUse = default;
    [SerializeField] protected UnityEvent<Player> m_onPickup = default;
    [SerializeField] protected TemporaryInputAction m_pickUpCommand = default;
    protected int m_stackCount = 1;
    protected bool m_isEffectActive = false;
    protected Player m_user;
    public ItemProperty Property => m_property;
    public int StackCount { get => m_stackCount; set => m_stackCount = value; }
    // Pickup trigger
    void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.attachedRigidbody == null) return;
        bool compExist = col.attachedRigidbody.TryGetComponent(out Player result);
        m_user = result;
        // when projectile hit this body, trigger on hit effects from projectile
        if (m_pickupImmediately && compExist)
        {
            OnPickup(result);
        }
        else if (!m_pickupImmediately && compExist)
        {
            m_pickUpCommand?.Enable();
        }
    }  
    // when player presses pickup for weapons
    public void PickupEquipment(InputAction.CallbackContext c) 
    {
        OnPickup(m_user);
    }
    public virtual void OnPickup(Player player)
    {
        m_onPickup?.Invoke(player);
        // despawn on pickup
        if (m_pickupImmediately) 
        {
            Destroy(gameObject);
        }
    }
    public virtual void UseItem(Player player)
    {
        m_isEffectActive = true;
        m_onUse?.Invoke(player);
    }
}