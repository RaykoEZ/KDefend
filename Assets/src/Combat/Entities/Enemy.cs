using System;
using System.Collections;
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
    protected Transform m_target;
    Coroutine m_movement;
    public event OnEnemyUpdate OnDefeated;
    public void Init(Transform target)
    {
        m_target = target;
        StartMoving();
    }
    public virtual void StartAttack(Vector2 dirNormalize) 
    {
    }
    protected void SwitchTarget(BaseEntity newTarget) 
    {
        if (newTarget == null) return;
        m_target = newTarget.transform;
    }
    public override void TakeDamage(int baseDamage)
    {
        // the lower the enemy hp, the greater the stun duration
        float stunDuration = BaseStats.Health / (CurrentStats.Health + 0.1f);
        stunDuration = Mathf.Clamp(stunDuration, 0.1f, 3f);
        StartCoroutine(HitStun(stunDuration));
        base.TakeDamage(baseDamage);
    }
    protected override void OnDefeat()
    {
        base.OnDefeat();
        OnDefeated?.Invoke(this);
        Destroy(gameObject);
    }
    public void StartMoving()
    {
        if (m_movement == null && m_target != null)
        {
            m_movement = StartCoroutine(Movement());
        }
    }
    public void StopMoving()
    {
        if (m_movement != null)
        {
            StopCoroutine(m_movement);
            m_movement = null;
        }
    }
    IEnumerator HitStun(float duration) 
    {
        StopMoving();
        yield return new WaitForSeconds(duration);
        StartMoving();
    }
    IEnumerator Movement()
    {
        float dist = Vector3.Distance(transform.position, m_target.position);
        float t = 0f;
        while (dist > 0.01f)
        {
            t += CurrentStats.MoveSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, m_target.position, t);
            yield return new WaitForSeconds(0.5f);
            dist = Vector3.Distance(transform.position, m_target.position);
        }
        m_movement = null;
    }

    public virtual void OnHit<T>(T hit) where T : BaseEntity
    {
        hit?.TakeDamage(m_contactDamage);
    }
}