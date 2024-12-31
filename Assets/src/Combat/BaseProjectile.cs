using System;
using System.Collections;
using UnityEngine;
using UnityEngine.WSA;
// Object shot from weapon
// has speed, direction, life, and on-hit effect
[Serializable]
public struct ProjectileProperty 
{
    public float Speed;
    public float Life;
    public int PierceCount;
    public int Damage;
}
public interface IWeapon 
{
    public void Fire(Vector2 direction);
}
[RequireComponent(typeof(Rigidbody2D))]
public class BaseProjectile : MonoBehaviour, IWeapon
{
    protected bool m_isFlying = false;
    protected float m_lifeTimer = 0f;
    [SerializeField] protected ProjectileProperty m_property = default;
    public ProjectileProperty Property => m_property;
    protected Vector2 m_direction;
    protected Rigidbody2D rb => GetComponent<Rigidbody2D>();
    void Update()
    {
        while (m_isFlying)
        {
            // move projectile
            rb.MovePosition(rb.position +
                (Time.deltaTime * m_property.Speed * m_direction));
            m_lifeTimer += Time.deltaTime;
            if (m_lifeTimer >= Property.Life)
            {
                m_isFlying = false;
                EndProjectile();
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BaseEntity result)) 
        {
            OnHit(result);
        }
    }
    public virtual void Fire(Vector2 directionNormalized) 
    {
        // start update
        m_direction = directionNormalized;
        m_isFlying = true;
    }
    protected virtual void OnHit(BaseEntity hit) 
    {
        hit.TakeDamage(Property.Damage);
        m_property.PierceCount--;
        // kill object when hitting a target, unless we pierce
        if (m_property.PierceCount < 0) 
        {
            m_isFlying = false;
        }
    }
    protected virtual void EndProjectile() 
    {
        Destroy(gameObject);
    }
}