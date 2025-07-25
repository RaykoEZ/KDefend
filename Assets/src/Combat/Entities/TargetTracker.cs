using System.Collections;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class TargetTracker : MonoBehaviour 
{
    [Range(100f, 10000f)]
    [SerializeField] float m_warpDistanceThreshold = default;
    [SerializeField] float m_teleportCooldownTime = default;
    [SerializeField] float m_updateTimeInterval = default;
    [SerializeField] FormationHandler m_formation = default;
    Vector2 m_precisePosition = Vector2.zero;
    BaseEntity m_target;
    Coroutine m_tracking;
    Coroutine m_teleportCooldown;
    public bool IsReady => m_target != null;
    public Vector2 PrecisePosition => m_precisePosition;
    public NavMeshAgent Navigator => GetComponent<NavMeshAgent>();
    void OnEnable()
    {
        m_teleportCooldown = StartCoroutine(Cooldown());
    }
    protected IEnumerator TrackTarget() 
    {
        while (m_target != null && Navigator.isActiveAndEnabled) 
        {
            // update current target destination
            m_precisePosition = m_target.transform.position;
            // try teleport near player if too far from destimation
            if (Navigator.remainingDistance > m_warpDistanceThreshold)
            {
                TryTeleportNearPlayer();
            }
            yield return new WaitForSeconds(m_updateTimeInterval);
        }
    }
    protected IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(m_teleportCooldownTime);
        m_teleportCooldown = null;
    }
    void TryTeleportNearPlayer()
    {
        if (m_teleportCooldown != null) return;
        if (TryGetWarpPosition(out Vector3 result))
        {
            m_teleportCooldown = StartCoroutine(Cooldown());
            Navigator?.Warp(result);
        }
    }
    bool TryGetWarpPosition(out Vector3 warpPosition) 
    {
        return m_formation.TryGetFormationPosition(m_target, out warpPosition);
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
    public virtual void ResetTarget()
    {
        if (m_tracking != null)
        {
            StopCoroutine(m_tracking);
            m_tracking = null;
        }
        m_target = null;
    }
}
