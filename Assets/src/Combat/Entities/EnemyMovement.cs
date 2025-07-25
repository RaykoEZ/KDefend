using Curry.Util;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy), typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour 
{
    [SerializeField] bool m_moveOnsight = default;
    [SerializeField] TargetTracker m_tracker = default;
    protected float m_speedVariant;
    protected Vector2 m_currentDestination;
    protected Vector2 m_defaultTarget;
    Coroutine m_movement;
    public NavMeshAgent Navigator => GetComponent<NavMeshAgent>();
    public bool MoveOnsight { get => m_moveOnsight; set => m_moveOnsight = value; }

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
        m_tracker?.UpdateTarget(defaultTarget);
    }
    public void StartMoving()
    {
        SetupNavigation();
        var nav = Navigator;
        m_currentDestination = m_tracker.IsReady? m_tracker.PrecisePosition : m_defaultTarget;
        nav?.SetDestination(m_currentDestination);
        // if enemy sees player, immediately reroute path to pusue player
        if (m_movement == null && gameObject.activeSelf)
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
    // move to a position, reset to not chase player
    public void MoveToPosition(Vector2 newTarget) 
    {
        m_defaultTarget = newTarget;
        ResetTarget();
    }
    public void UpdateTarget(BaseEntity newTarget)
    {
        if (newTarget == null) return;
        m_tracker?.UpdateTarget(newTarget);
        if (m_moveOnsight)
        {
            // if not moving, start chasing
            StartMoving();
        }
    }
    public virtual void ResetTarget()
    {
        float duration = UnityEngine.Random.Range(0.5f, 1f);
        StartCoroutine(Standby(duration));
    }
    protected virtual IEnumerator Movement()
    {
        var nav = Navigator;
        float dist = Vector2.Distance(transform.position, m_currentDestination);
        float waitTime;
        while (dist > nav.stoppingDistance)
        {
            m_currentDestination = m_tracker.IsReady? m_tracker.PrecisePosition : m_defaultTarget;
            // the farther we are from target, the longer our path refresh interval
            waitTime = Mathf.Clamp(0.1f * (dist / 100f), 0.1f, 5f);
            nav?.SetDestination(m_currentDestination);
            yield return new WaitForSeconds(waitTime);
            dist = Navigator.remainingDistance;
        }
        m_movement = null;
    }
    protected virtual IEnumerator Standby(float duration)
    {
        StopMoving();
        yield return new WaitForSeconds(duration);
        m_tracker?.ResetTarget();
        Navigator?.SetDestination(m_defaultTarget);
        StartMoving();
    }
}
