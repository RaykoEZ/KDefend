using System;
using System.Collections;
using UnityEngine;
using UnityEngine.WSA;
// Object shot from weapon
// has speed, direction, life, and on-hit effect
[Serializable]
public struct ProjectileProperty 
{
    public int Damage;
    public int PierceCount;
    public int ShotsPerCycle;
    public float Speed;
    public float Life;
    public float DelayPerShot;
    public float DelayPerCycle;
}
[RequireComponent(typeof(Rigidbody2D))]
public class BaseProjectile : MonoBehaviour
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
    protected static T NewProjectileInstance<T>(T prefabRef, Transform parent) where T:BaseProjectile
    {
        return Instantiate(prefabRef, parent, true);
    }
    // call to fire off a projectile
    public static void Fire<T>(T prefabRef, Transform parent, Vector2 directionNormalized) where T : BaseProjectile
    {
        T proj = NewProjectileInstance(prefabRef, parent);
        proj?.Launch(directionNormalized);
    }
    protected virtual void Launch(Vector2 directionNormalized) 
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