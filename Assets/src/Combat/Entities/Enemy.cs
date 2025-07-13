using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Curry.Game;
using System;
// base enemy behaviour
public delegate void OnEnemyUpdate(Enemy toUpdate);
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : BaseCharacter, IHitsEntity
{
    [SerializeField] bool m_autoAttack = default;
    [SerializeField] int m_type = default;
    [SerializeField] int m_contactDamage = default;
    [Range(-100, 100)]
    [SerializeField] protected int m_threatIncrease = default;
    [SerializeField] protected EnemyMovement m_movementHandler = default;
    [SerializeField] protected RangeDetector m_targeting = default;
    protected BaseEntity m_target;
    Coroutine m_attack;
    public int ThreatIncrease => m_threatIncrease;
    public BaseEntity CurrentTarget { get => m_target; }
    public EnemyMovement Navigator => m_movementHandler;
    public EnemyState State => 
        new EnemyState { 
            EnemyIndex = m_type, 
            State = CurrentStats };

    public event OnEnemyUpdate OnDefeated;
    protected virtual void Update() 
    {
        if (m_autoAttack && m_attack == null && m_targeting.IsInRange(m_target)) 
        {
            UseWeapon();
        }
        else 
        {
            m_keepFiring = false;
        }
    }
    protected override Vector2 GetAimDirection()
    {
        return m_target == null? Vector2.zero : (m_target.transform.position - transform.position).normalized;
    }
    public virtual void InitTarget(BaseEntity defaultTarget = null)
    {
        base.Init(BaseStats);
        EnemyAggroHandler.Add(this);
        m_target = defaultTarget;
        m_movementHandler?.Init(defaultTarget);
        m_movementHandler?.StartMoving();
    }
    public void SetAggro(bool enable = true) 
    {
        m_targeting.enabled = enable;
    }
    public void UpdateTarget(BaseEntity newTarget) 
    {
        if (newTarget == null) return;
        m_target = newTarget;
    }
    public override void TakeDamage(int baseDamage)
    {
        // the lower the enemy hp, the greater the stun duration
        float stunDuration = UnityEngine.Random.Range(0.2f, 0.5f);
        StartCoroutine(HitStun(stunDuration));
        base.TakeDamage(baseDamage);
    }
    protected override void OnDefeat()
    {
        StartCoroutine(Defeat_Internal());
    }
    protected virtual IEnumerator Defeat_Internal() 
    {
        m_movementHandler?.StopMoving();
        base.OnDefeat();
        OnDefeated?.Invoke(this);
        yield return new WaitForSeconds(0.5f);
        Despawn();
    }
    public void Despawn() 
    {
        m_movementHandler?.StopMoving();
        EnemyAggroHandler.Remove(this);
        if (TryGetComponent(out IPoolable poolable)) 
        {
            poolable?.ReturnToPool();
        }
    }
    protected virtual IEnumerator HitStun(float duration) 
    {
        m_movementHandler?.StopMoving();
        m_keepFiring = false;
        yield return new WaitForSeconds(duration);
        if (m_current.Health <= 0) yield break;
        m_movementHandler?.StartMoving();
        if (m_autoAttack) 
        {
            UseWeapon();
        }
    }
    public override void UseWeapon()
    {
        if (m_currentWeapons.Count == 0) return;
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
            for (int i = 0; i < m_currentWeapons.Count; i++)
            {
                // unleash one instance of a weapon's attack, include its recovery frames
                yield return Attack_Internal(m_currentWeapons[i]);
            }
            // atop attacking loop if we don't auto fire
            if (!m_autoAttack) 
            {
                m_keepFiring = false;
            }
        }
        m_attack = null;
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
    internal void ResetTarget()
    {
        m_target = null;
        m_movementHandler?.ResetTarget();
    }
}