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
    [NonSerialized] int m_type = default;
    [SerializeField] int m_contactDamage = default;
    [Range(0f, 1f)]
    [SerializeField] float m_hitStunMod = default;
    [Range(-100, 100)]
    [SerializeField] protected int m_threatIncrease = default;
    [SerializeField] protected EnemyMovement m_movementHandler = default;
    public int ThreatIncrease => m_threatIncrease;
    public EnemyMovement Navigator => m_movementHandler;
    public EnemyState State => 
        new EnemyState { 
            EnemyIndex = m_type, 
            State = CurrentStats };

    public event OnEnemyUpdate OnDefeated;
    public void SetEnemyType(int value)
    {
        m_type = value;
    }
    public virtual void InitTarget(BaseEntity defaultTarget = null)
    {
        base.Init(BaseStats);

        EnemyAggroHandler.Add(this);
        m_movementHandler?.Init(defaultTarget);
        m_movementHandler?.StartMoving();
    }
    public void SetAggro(bool enable = true) 
    {
        m_movementHandler?.ResetTarget();
        (m_attackHandler as NpcAttackHandler)?.ResetTarget();
    }
    public override void TakeDamage(int baseDamage)
    {
        // the lower the enemy hp, the greater the stun duration
        StartCoroutine(HitStun());
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
    protected virtual IEnumerator HitStun() 
    {
        if (Mathf.Approximately(m_hitStunMod, 0f)) yield break;
        float stunDuration = UnityEngine.Random.Range(0.1f, 1f);
        m_movementHandler?.StopMoving();
        m_attackHandler.KeepFiring = false;
        yield return new WaitForSeconds(stunDuration * m_hitStunMod);
        if (m_current.Health <= 0) yield break;
        m_movementHandler?.StartMoving();
        NpcAttackHandler attack = m_attackHandler as NpcAttackHandler;
        if (attack.AutoAttack) 
        {
            m_attackHandler?.UseWeapon();
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
            StartCoroutine(HitStun());
        }
    }
    internal void ResetTarget()
    {
        m_movementHandler?.ResetTarget();
        (m_attackHandler as NpcAttackHandler)?.ResetTarget();
    }
}