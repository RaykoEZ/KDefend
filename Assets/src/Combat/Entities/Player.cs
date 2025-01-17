using UnityEngine;
using UnityEngine.InputSystem;

public class Player : BaseCharacter 
{
    [SerializeField] MeleeAttack m_bat = default;
    [SerializeField] BaseProjectile m_shoot = default;
    void Start()
    {
        m_currentWeapon = m_shoot;
    }
    // turn off firing
    void Update()
    {
        m_firing = Mouse.current.leftButton.isPressed;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherRigidbody == null) return;
        // when projectile hit this body, trigger on hit effects from projectile
        if (collision.otherRigidbody.TryGetComponent(out IHitsEntity result))
        {
            result?.OnHit(this);
        }
    }
    // Trigger this when player clicks to fire weapon
    public void OnFire() 
    {
        UseWeapon();
    }

    // get current aiming direction
    protected override Vector2 GetAimDirection()
    {
        Vector2 world = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 pos = new Vector2(rb.position.x, rb.position.y);
        return (world - pos).normalized;
    }
}