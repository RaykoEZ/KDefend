using UnityEngine;
using UnityEngine.InputSystem;
public class Player : BaseCharacter 
{
    DeliveryDetail m_currentlyDelivering;
    public DeliveryDetail CurrentlyDelivering { get => m_currentlyDelivering; set => m_currentlyDelivering = value; }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherRigidbody == null) return;
        // when projectile hit this body, trigger on hit effects from projectile
        if (collision.otherRigidbody.TryGetComponent(out IHitsEntity result))
        {
            result?.OnHit(this);
        }
    }
}