using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public enum NavigationMode 
{ 
    Seek,
}
// output position of tracked entity or follow formation
[RequireComponent(typeof(NavMeshAgent))]
public class TargetTracker : MonoBehaviour 
{
    [Range(100f, 10000f)]
    [SerializeField] float m_warpDistanceThreshold = default;
    [SerializeField] float m_teleportCooldownTime = default;
    [SerializeField] float m_updateTimeInterval = default;
    [SerializeField] NavigationMode m_navMode = default;
    [SerializeField] EncircleHandler m_encircle = default;
    protected Vector2 m_currentTarget;
    protected Vector2 m_defaultTarget;
    BaseEntity m_target;
    Coroutine m_tracking;
    Coroutine m_teleportCooldown;
    public bool IsReady => m_target != null;
    public NavMeshAgent Navigator => GetComponent<NavMeshAgent>();
    public NavigationMode Mode { get => m_navMode; set => m_navMode = value; }
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
    public void ResetTarget() 
    {
        m_currentTarget = m_defaultTarget;
    }
    protected IEnumerator TrackTarget()
    {
        while (m_target != null && Navigator.isOnNavMesh) 
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
        return m_encircle == null? false : m_encircle.TryGetFormationPosition(m_target, out warpPosition);
    }
}
