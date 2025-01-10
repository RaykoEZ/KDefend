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
public class Hostile : BaseCharacter 
{
    [SerializeField] float m_moveInterval = default;
    protected Transform m_target;
    Coroutine m_movement;
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
    protected override void OnDefeat()
    {
        StopMoving();
        base.OnDefeat();
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
    IEnumerator Movement()
    {
        float dist = Vector3.Distance(transform.position, m_target.position);
        float t = 0f;
        while (dist > 0.01f)
        {
            t += m_moveInterval * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, m_target.position, t);
            yield return new WaitForSeconds(0.5f);
            dist = Vector3.Distance(transform.position, m_target.position);
        }
        m_movement = null;
    }
}