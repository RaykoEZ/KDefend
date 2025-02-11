using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemySpawnPattern
{
    public int KillsForEarlySpawn;
    public float SecondsElapsed;
}
[Serializable]
public struct EntityState 
{
    public EntityProperty Property;
    public Vector2 Position;
}
// base enemy behaviour
public delegate void OnEnemyUpdate(Enemy toUpdate);
public class Enemy : BaseCharacter, IHitsEntity
{
    [SerializeField] int m_contactDamage = default;
    [SerializeField] float m_separationFromTarget = default;
    [SerializeField] RangeDetector m_targeting = default;
    [SerializeField] BaseEntity m_defaultTEST = default;
    protected int m_targetPriority = -1;
    protected float m_speedVariant;
    private BaseEntity m_defaultTarget;
    protected Transform m_target;
    Coroutine m_movement;
    Coroutine m_attack;
    public IReadOnlyList<BaseEntity> TargetsOfInterest => m_targeting.TargetPriorityList;
    public BaseEntity DefaultTarget { get => m_defaultTarget; }
    public event OnEnemyUpdate OnDefeated;
    void Start() 
    {
        Init(new List<BaseEntity>(), m_defaultTEST);
    }
    protected override Vector2 GetAimDirection()
    {
        return m_target == null? Vector2.zero : (m_target.position - transform.position).normalized;
    }
    public void Init(List<BaseEntity> interests, BaseEntity defaultTarget)
    {
        m_targeting?.AddTargets(interests);
        m_defaultTarget = defaultTarget;
        m_target = defaultTarget.transform;
        m_speedVariant = UnityEngine.Random.Range(0.8f, 1.1f);
        StartMoving();
    }
    public void UpdateTarget(BaseEntity newTarget) 
    {
        if (newTarget == null) return;
        m_target = newTarget.transform;
        // if not moving, start chasing
        StartMoving();
    }
    public void OnLosingTarget() 
    {
        m_keepFiring = false;
        StopCoroutine(m_attack);
        m_attack = null;
        // if not moving, start chasing
        StartMoving();
    }
    public override void TakeDamage(int baseDamage)
    {
        // the lower the enemy hp, the greater the stun duration
        float stunDuration = UnityEngine.Random.Range(0.05f, 0.1f);
        StartCoroutine(HitStun(stunDuration));
        base.TakeDamage(baseDamage);
    }
    protected override void OnDefeat()
    {
        base.OnDefeat();
        OnDefeated?.Invoke(this);
        Destroy(gameObject);
    }
    public virtual void StartMoving()
    {
        if (m_movement == null && m_target != null)
        {
            m_movement = StartCoroutine(Movement());
        }
    }
    public virtual void StopMoving()
    {
        if (m_movement != null)
        {
            StopCoroutine(m_movement);
            m_movement = null;
        }
    }
    public void ResetTarget() 
    {
        m_target = m_defaultTarget.transform;
    }
    protected virtual IEnumerator HitStun(float duration) 
    {
        StopMoving();
        yield return new WaitForSeconds(duration);
        StartMoving();
    }
    protected virtual IEnumerator Movement()
    {
        float dist = Vector3.Distance(transform.position, m_target.position);
        float t = 0f;
        while (dist > m_separationFromTarget)
        {
            t += CurrentStats.MoveSpeed * m_speedVariant * 0.00001f * Time.deltaTime;
            transform.position =
                Vector3.Lerp(transform.position, m_target.position, t);
            yield return new WaitForEndOfFrame();
            dist = Vector3.Distance(transform.position, m_target.position);
        }
        UseWeapon();
        m_movement = null;
    }
    public override void UseWeapon()
    {
        if (m_attack == null)
        {
            m_keepFiring = true;
            m_attack = StartCoroutine(AttackCycle_Sequence());
        }
    }
    // make attack pattern sequential vs silmultaneous
    IEnumerator AttackCycle_Sequence() 
    {
        while (m_keepFiring)
        {
            for (int i = 0; i < m_weapons.Count; i++)
            {
                yield return Attack_Internal(m_weapons[i]);
            }
        }
    }
    // contact damage
    public virtual void OnHit<T>(T hit) where T : BaseEntity
    {
        if ( hit is Player || hit is PlayerBase)
        {
            hit?.TakeDamage(m_contactDamage);
        }
        Vector2 dir = hit.transform.position - transform.position;
        if (hit is IPushable push && !(hit is Enemy))
        {
            push.Push(dir.normalized, 0.25f);
            Push(-dir.normalized, 0.25f);
            StartCoroutine(HitStun(0.25f));
        }
    }
}