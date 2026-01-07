using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public enum NavigationMode 
{ 
    Seek,
    Formation
}
// output position of tracked entity or follow formation
[RequireComponent(typeof(NavMeshAgent))]
public class TargetTracker : MonoBehaviour 
{
    [Range(100f, 10000f)]
    [SerializeField] float m_warpDistanceThreshold = default;
    [SerializeField] float m_teleportCooldownTime = default;
    [SerializeField] float m_updateTimeInterval = default;
    [SerializeField] Formation m_defaultFormation = default;
    [SerializeField] NavigationMode m_navMode = default;
    protected Vector2 m_currentTarget;
    protected Vector2 m_defaultTarget;
    BaseEntity m_target;
    Coroutine m_tracking;
    Coroutine m_teleportCooldown;
    Formation m_currentFormationRef;
    public bool IsReady => m_target != null || m_currentFormationRef != null;
    public Vector2 DefaultTarget { get => m_defaultTarget; set => m_defaultTarget = value; }
    public NavMeshAgent Navigator => GetComponent<NavMeshAgent>();
    public NavigationMode Mode { get => m_navMode; set => m_navMode = value; }
    void Start()
    {
        SetNewFormation(m_defaultFormation);
    }
    void OnEnable()
    {
        m_teleportCooldown = StartCoroutine(Cooldown());
        m_defaultTarget = transform.position;
        m_currentTarget = transform.position;
    }
    public Vector2 GetPrecisePosition()
    {
        switch (m_navMode)
        {
            case NavigationMode.Seek:
                return m_currentTarget;
            case NavigationMode.Formation:
                m_currentFormationRef.TryGetFormationPosition(m_target, out Vector3 ret);
                m_currentTarget = ret;
                break;
            default:
                break;
        }
        return transform.position;
    }
    public void UpdateTarget(BaseEntity newTarget)
    {
        if (newTarget == null) return;
        m_target = newTarget;
        if (m_tracking != null)
        {
            StopCoroutine(m_tracking);
        }
        m_tracking = StartCoroutine(TrackTarget());
    }
    public void SetNewFormation(Formation newFormation) 
    {
        if (newFormation == null) return;
        if (m_currentFormationRef != null) 
        {
            m_currentFormationRef.OnFormationEnd -= OnFormationEnd;
        }
        m_currentFormationRef = newFormation;
        m_currentFormationRef.OnFormationEnd += OnFormationEnd;
    }
    public void OnFormationEnd() 
    { 
        m_navMode = NavigationMode.Seek;
    }
    public void ResetFormation() 
    {
        m_currentFormationRef = m_defaultFormation;
    }
    protected IEnumerator TrackTarget()
    {
        while (m_target != null && Navigator.isActiveAndEnabled) 
        {
            // update current target destination
            m_currentTarget = m_target.transform.position;
            float directDist = Vector2.Distance(m_currentTarget, transform.position);
            // try teleport near player if too far from destimation
            if (Navigator.remainingDistance > m_warpDistanceThreshold && 
                directDist > m_warpDistanceThreshold)
            {
                TryTeleportNearTarget();
            }
            yield return new WaitForSeconds(m_updateTimeInterval);
        }
    }
    protected IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(m_teleportCooldownTime);
        m_teleportCooldown = null;
    }
    void TryTeleportNearTarget()
    {
        if (m_teleportCooldown != null) return;
        // if we are seeking target and too far away, try warp
        if (m_navMode == NavigationMode.Seek && TryGetWarpPosition(out Vector3 result))
        {
            m_teleportCooldown = StartCoroutine(Cooldown());
            Navigator?.Warp(result);
        }
    }
    bool TryGetWarpPosition(out Vector3 warpPosition) 
    {
        warpPosition = m_target.transform.position;
        return m_currentFormationRef == null? false : m_currentFormationRef.TryGetFormationPosition(m_target, out warpPosition);
    }
}
