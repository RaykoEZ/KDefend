using System;
using System.Collections;
using UnityEngine;
// Object shot from weapon
// has speed, direction, life, and on-hit effect
[Serializable]
public struct WeaponProperty 
{
    public bool PassWalls;
    public int Damage;
    public int AttackPerCycle;
    public float Speed;
    public float Life;
    public float DelayPerAttack;
    public float DelayPerCycle;
    public float PushPower;
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
    protected Rigidbody2D rb => GetComponent<Rigidbody2D>();
    public override bool InstantiateWeapon => true;
    protected virtual void OnTriggerEnter2D(Collider2D c) 
    {
        string layerName = LayerMask.LayerToName(c.gameObject.layer);
        if (!WeaponProperty.PassWalls && layerName == "Building") 
        {
            // end flying loop and cleanup
            StopAllCoroutines();
            EndProjectile();
        }
    }
    IEnumerator Flying()
    {
        while (m_isFlying)
        {
            // move projectile
            rb.MovePosition(rb.position +
                (Time.deltaTime * m_weaponProperty.Speed * m_currentDirection));
            yield return new WaitForFixedUpdate();
            m_lifeTimer += Time.fixedDeltaTime;
            if (m_lifeTimer >= WeaponProperty.Life)
            {
                m_isFlying = false;
            }
        }
        // stop flying loop and cleanup
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
        m_currentDirection = directionNormalized;
        m_isFlying = true;
        StartCoroutine(Flying());
    }
    protected virtual void EndProjectile() 
    {
        m_isFlying = false;
        Destroy(gameObject);
    }
}