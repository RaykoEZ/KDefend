using System;
using UnityEngine;
public class Player : BaseCharacter 
{
    [SerializeField] InventoryManager m_inventory = default;
    [SerializeField] AudioSource m_contactDamageAudio = default;
    DeliveryDetail m_currentlyDelivering;
    public DeliveryDetail CurrentlyDelivering { get => m_currentlyDelivering; set => m_currentlyDelivering = value; }
    public InventoryManager Inventory => m_inventory;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherRigidbody == null) return;
        // when projectile hit this body, trigger on hit effects from projectile
        if (collision.otherRigidbody.TryGetComponent(out IHitsEntity result))
        {
            result?.OnHit(this);
        }
    }
    public void ContactDamage(int damage) 
    {
        TakeDamage(damage);
        m_contactDamageAudio?.Play();
    }
    public void ResetFromDeath()
    {
        m_current.Health = 0;
        Heal(BaseStats.Property.Health);
    }
}