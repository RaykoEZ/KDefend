using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Curry.Game;
using System;
public interface IMovement 
{
    void StartMoving();
    void StopMoving();
}
// base enemy behaviour
public delegate void OnEnemyUpdate(Enemy toUpdate);
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : BaseCharacter, IHitsEntity
{
    [NonSerialized] int m_type = default;
    [SerializeField] int m_contactDamage = default;
    [SerializeField] protected NpcMovement m_movementHandler = default;
    public NpcMovement Navigator => m_movementHandler;
    public override IMovement Movement => m_movementHandler;
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
    public virtual void OnHit<T>(T hit) where T : BaseEntity
    {
        if (hit is Player)
        {
            // contact damage
            hit?.TakeDamage(m_contactDamage);
        }
        Vector2 dir = hit.transform.position - transform.position;
        if (hit is IPushable push && !(hit is Enemy))
        {
            push.Push(dir.normalized, 0.25f);
            Push(-dir.normalized, 0.25f);
        }
    }
    internal void ResetTarget()
    {
        m_movementHandler?.ResetTarget();
        (m_attackHandler as NpcAttackHandler)?.ResetTarget();
    }
}