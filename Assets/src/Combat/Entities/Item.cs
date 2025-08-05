using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[Serializable]
public struct ItemProperty
{
    public string Name;
    public string Description;
}

public interface IItem
{
    ItemProperty Property { get; }
    // Use as counter from item amount/level
    int StackCount { get; set; }
    public void UseItem();
    public void OnPickup();
}
[RequireComponent(typeof(Collider2D))]
// class to contain item property and interaction triggers
public class Item : MonoBehaviour , IItem
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
            OnPickup();
        }
        else if (!m_pickupImmediately && compExist)
        {
            m_pickUpCommand?.Enable();
        }
    }
    void OnTriggerExit2D() 
    {
        m_pickUpCommand?.Disable();
    }
    // when player presses pickup for weapons
    public virtual void PickupDrop(InputAction.CallbackContext _) 
    {
        OnPickup();
    }
    public virtual void OnPickup()
    {
        if (m_user == null) return;
        m_onPickup?.Invoke(m_user);
        m_pickUpCommand?.Disable();
        Destroy(gameObject);
    }
    public virtual void UseItem()
    {
        m_isEffectActive = true;
        m_onUse?.Invoke(m_user);
    }
}
