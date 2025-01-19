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
public struct EntityState 
{
    public int Hp;
    public Vector2 Position;
}
public delegate void OnEnemyUpdate(Enemy toUpdate);
public class Enemy : BaseCharacter, IHitsEntity
{
    [SerializeField] int m_contactDamage = default;
    [SerializeField] EnemyTargetFinder m_targeting = default;
    protected int m_targetPriority = -1;
    protected float m_speedVariant;
    protected BaseEntity m_defaultTarget;
    protected Transform m_target;
    Coroutine m_movement;
    public event OnEnemyUpdate OnDefeated;
    public void Init(List<BaseEntity> interests, BaseEntity defaultTarget)
    {
        m_targeting?.AddTargets(interests);
        m_defaultTarget = defaultTarget;
        m_target = defaultTarget.transform;
        m_speedVariant = UnityEngine.Random.Range(0.8f, 1.15f);
        StartMoving();
    }
    public void UpdateTarget(BaseEntity newTarget) 
    {
        if (newTarget == null) return;
        m_target = newTarget.transform;
    }
    public void TargetLost() 
    {
        UpdateTarget(m_defaultTarget);
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
        while (dist > 0.01f)
        {
            t += CurrentStats.MoveSpeed * m_speedVariant * 0.005f * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, m_target.position, t);
            yield return new WaitForEndOfFrame();
            dist = Vector3.Distance(transform.position, m_target.position);
        }
        m_movement = null;
    }
    // contact damage
    public virtual void OnHit<T>(T hit) where T : BaseEntity
    {
        if ( hit is Player || hit is PlayerBase)
        {
            hit?.TakeDamage(m_contactDamage);
        }
        Vector2 dir = hit.transform.position - transform.position;
        if (hit is IPushable push)
        {
            push.Push(dir.normalized, 0.25f);
        }
        Push(-dir.normalized, 0.25f);
        StartCoroutine(HitStun(0.25f));
    }
}