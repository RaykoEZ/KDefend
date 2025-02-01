using System;
using UnityEngine;
using UnityEngine.Events;
using static Cinemachine.CinemachineOrbitalTransposer;

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
    public void Activate();
    public void Deactivate();
    public void OnPickup();
}
[RequireComponent(typeof(Collider2D))]
// class to contain item property and interaction triggers
public class Item : MonoBehaviour , IItem
{
    [SerializeField] protected ItemProperty m_property = default;
    [SerializeField] protected UnityEvent<Item> m_onUse = default;
    [SerializeField] protected UnityEvent<Item> m_onPickup = default;
    protected int m_stackCount = 1;
    protected bool m_isEffectActive = false;
    protected Player m_heldBy;
    public ItemProperty Property => m_property;
    public int StackCount { get => m_stackCount; set => m_stackCount = value; }
    // Pickup trigger
    void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.attachedRigidbody == null) return;
        // when projectile hit this body, trigger on hit effects from projectile
        if (col.attachedRigidbody.TryGetComponent(out Player result))
        {
            m_heldBy = result;
            OnPickup();
        }
    }  
    public virtual void UpdateState(KDefenderEventContext e) 
    {
    }
    public virtual void OnPickup()
    {
        m_onPickup?.Invoke(this);
    }
    public virtual void Activate()
    {
        m_isEffectActive = true;
        m_onUse?.Invoke(this);
    }

    public virtual void Deactivate()
    {
        m_isEffectActive = false;
    }
}

public class Consumable : Item 
{
    public override void OnPickup()
    {
        Activate();
        base.OnPickup();
    }
}
public class WeaponDrop : Item 
{
    [SerializeField] BaseWeapon m_weaponRef = default;
    public override void OnPickup()
    {
        m_heldBy?.AddWeapon(m_weaponRef);
        base.OnPickup();
    }
}
public class Collectible : Item 
{
    public override void UpdateState(KDefenderEventContext e)
    {
        bool check = MatchActivationCondition(e);
        if (check) 
        {
            Activate();
        }
        else 
        {
            Deactivate();
        }
    }
    protected virtual bool MatchActivationCondition(KDefenderEventContext e) 
    {
        return true;
    }
}