using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// for controlling NPC movement with navigation system and position tracking
[RequireComponent(typeof(NavMeshAgent))]
public class NpcMovement : MonoBehaviour, IMovement
{
    [SerializeField] protected bool m_moveOnsight = default;
    [SerializeField] protected bool m_useDirectDestination = default;
    [SerializeField] TargetTracker m_tracker = default;
    protected float m_speedVariant;
    // target node position of the next destination (not the chase target)
    protected Vector2 m_currentDestination;
    // use this when moving while ignoring player position tracking
    private Vector2 m_origin;
    Coroutine m_movement;
    public bool MoveOnsight { get => m_moveOnsight; set => m_moveOnsight = value; }
    public bool UseDirectDestination { get => m_useDirectDestination; set => m_useDirectDestination = value; }
    public Vector2 Origin { get => m_origin; set => m_origin = value; }
    public NavMeshAgent Navigator => GetComponent<NavMeshAgent>();

    public Vector2 DirectionNormalized => (Navigator.nextPosition - transform.position).normalized;
    public Vector2 Position => transform.position;

    void FixedUpdate()
    {
        if (m_movement == null && m_moveOnsight) 
        {
            StartMoving();
        }
    }
    void OnEnable()
    {
        m_origin = transform.position;
        SetupNavigation();
    }
    void SetupNavigation()
    {
        var nav = Navigator;
        // randomize avoidance priority for agent avoidance from each other
        nav.avoidancePriority = Random.Range(0, 64);
        nav.updateRotation = false;
        nav.updateUpAxis = false;
        nav.enabled = true;
    }
    public void Init(BaseEntity target = null)
    {
        m_speedVariant = Random.Range(0.75f, 1.25f);
        m_tracker?.UpdateTarget(target);
        UpdateTarget(target);
    }
    public void StartMoving()
    {
        if (!gameObject.activeSelf) return;
        if (m_movement != null)
        {
            StopCoroutine(m_movement);
        }
        // if enemy sees player, immediately reroute path to pusue player       
        SetupNavigation();
        Navigator.isStopped = false;
        m_movement = StartCoroutine(Movement());
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
        m_useDirectDestination = true;
        m_currentDestination = newTarget;
        if (m_movement == null) 
        {
            StartMoving();
        }
    }
    public void UpdateTarget(BaseEntity newTarget)
    {
        if (newTarget == null) return;
        m_tracker?.UpdateTarget(newTarget);
        m_currentDestination = newTarget.transform.position;
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
        m_currentDestination = GetDestination();
        float dist = Vector2.Distance(transform.position, m_currentDestination);
        float waitTime;
        while (dist > nav.stoppingDistance)
        {
            m_currentDestination = GetDestination();

            nav?.SetDestination(m_currentDestination);
            // the farther we are from target, the longer our path refresh interval
            waitTime = Mathf.Clamp(0.1f * (dist / 100f), 0.1f, 5f);
            yield return new WaitForSeconds(waitTime);
            dist = Navigator.remainingDistance;
        }
        m_movement = null;
    }
    protected Vector2 GetDestination() 
    {
        if (UseDirectDestination) return m_currentDestination;

        Vector2 ret = m_tracker.IsReady ? m_tracker.PrecisePosition : transform.position;
        return ret;
    }
    protected virtual IEnumerator Standby(float duration)
    {
        StopMoving();
        yield return new WaitForSeconds(duration);
        Navigator?.SetDestination(m_tracker.PrecisePosition);
        StartMoving();
    }
}
