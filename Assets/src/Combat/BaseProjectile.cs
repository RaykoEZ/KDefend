using System;
using System.Collections;
using UnityEngine;
// Object shot from weapon
// has speed, direction, life, and on-hit effect
[Serializable]
public struct WeaponProperty 
{
    public int Damage;
    public int AttackPerCycle;
    public float Speed;
    public float Life;
    public float DelayPerAttack;
    public float DelayPerCycle;
}
public interface IHitsEntity
{
    public void OnHit<T>(T hit) where T : BaseEntity;
}
[RequireComponent(typeof(Rigidbody2D))]
public class BaseProjectile : BaseWeapon, IHitsEntity
{
    protected bool m_isFlying = true;
    protected float m_lifeTimer = 0f;
    protected Vector2 m_direction;
    protected Rigidbody2D rb => GetComponent<Rigidbody2D>();
    IEnumerator Flying()
    {
        while (m_isFlying)
        {
            // move projectile
            rb.MovePosition(rb.position +
                (Time.deltaTime * m_property.Speed * m_direction));
            yield return new WaitForEndOfFrame();
            m_lifeTimer += Time.fixedDeltaTime;
            if (m_lifeTimer >= Property.Life)
            {
                m_isFlying = false;
                EndProjectile();
            }
        }
        EndProjectile();
    }
    public override void OnHit<T>(T hit)
    {
        base.OnHit(hit);
        // kill object when hitting a target, unless we pierce
        m_isFlying = false;
    }
    // call to fire off a projectile
    protected override void LaunchAttack(Vector2 directionNormalized) 
    {
        // start update
        m_direction = directionNormalized;
        m_isFlying = true;
        StartCoroutine(Flying());
    }
    protected virtual void EndProjectile() 
    {
        Destroy(gameObject);
    }
}