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
    [Range(0, 60)]
    [SerializeField] int m_spreadAngleRange = default;
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
    protected virtual IEnumerator InProgress()
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
    protected override Vector2 ModifyAttackDirection(Vector2 directionNormalized)
    {
        if (m_spreadAngleRange == 0) return directionNormalized;
        float rot = UnityEngine.Random.Range(-m_spreadAngleRange, m_spreadAngleRange);
        return (Quaternion.AngleAxis(rot, Vector3.forward) * directionNormalized);
    }
    public override void OnHit<T>(T hit)
    {
        base.OnHit(hit);
        // kill object when hitting a target, unless we pierce
        m_isFlying = false;
    }
    public void Reflect(LayerMask newAttackMask) 
    {
        if (!m_isFlying) return;
        gameObject.layer = newAttackMask;
        LaunchAttack(-m_currentDirection);
    }
    // call to fire off a projectile
    public override void LaunchAttack(Vector2 directionNormalized) 
    {
        if (m_isFlying) 
        {
            StopAllCoroutines();
        }
        // start update
        m_currentDirection = directionNormalized;
        m_isFlying = true;
        StartCoroutine(InProgress());
    }
    protected virtual void EndProjectile() 
    {
        m_isFlying = false;
        Destroy(gameObject);
    }
}