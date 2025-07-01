using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour 
{
    protected float m_speedVariant;
    protected Vector2 m_currentDestination;
    protected Vector2 m_defaultTarget;
    Coroutine m_movement;
    protected BaseEntity m_target;
    public NavMeshAgent Navigator => GetComponent<NavMeshAgent>();
    void SetupNavigation()
    {
        var nav = Navigator;
        nav.updateRotation = false;
        nav.updateUpAxis = false;
        nav.enabled = true;
    }
    public void Init(BaseEntity defaultTarget = null)
    {
        m_speedVariant = Random.Range(0.75f, 1.25f);
        m_defaultTarget = defaultTarget == null ? transform.position : defaultTarget.transform.position;
    }
    public void StartMoving()
    {
        if (!gameObject.activeSelf) return;
        SetupNavigation();
        var nav = Navigator;
        m_currentDestination = m_target == null ? m_defaultTarget : m_target.transform.position;
        nav?.SetDestination(m_currentDestination);
        // if enemy sees player, immediately reroute path to pusue player
        if (m_movement == null)
        {
            Navigator.enabled = true;
            Navigator.isStopped = false;
            m_movement = StartCoroutine(Movement());
        }      
    }
    public void StopMoving()
    {
        if (m_movement == null) return;
        StopCoroutine(m_movement);
        Navigator.velocity = Vector3.zero;
        Navigator.isStopped = true;
        m_movement = null;
        Navigator.enabled = false;
    }
    public void UpdateTarget(BaseEntity newTarget)
    {
        if (newTarget == null) return;
        m_target = newTarget;
        // if not moving, start chasing
        StartMoving();
    }
    public virtual void ResetTarget()
    {
        float duration = UnityEngine.Random.Range(1f, 5f);
        StartCoroutine(Standby(duration));
    }
    protected virtual IEnumerator Movement()
    {
        var nav = Navigator;
        float dist = Vector2.Distance(transform.position, m_currentDestination);
        float waitTime;
        while (dist > nav.stoppingDistance)
        {
            m_currentDestination = m_target == null ? m_defaultTarget : m_target.transform.position;
            // the farther we are from target, the longer our path refresh interval
            waitTime = Mathf.Clamp(0.1f * (dist / 100f), 0.1f, 5f);
            nav?.SetDestination(m_currentDestination);
            yield return new WaitForSeconds(waitTime);
            dist = Vector2.Distance(transform.position, m_currentDestination);
        }
        m_movement = null;
    }
    protected virtual IEnumerator Standby(float duration)
    {
        StopMoving();
        yield return new WaitForSeconds(duration);
        m_target = null;
        Navigator?.SetDestination(m_defaultTarget);
    }
}
