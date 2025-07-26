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
    protected Vector2 m_precisePosition;
    protected Vector2 m_defaultTarget;
    BaseEntity m_target;
    Coroutine m_tracking;
    Coroutine m_teleportCooldown;
    public bool IsReady => m_target != null;
    public Vector2 PrecisePosition => m_precisePosition;
    public Vector2 DefaultTarget { get => m_defaultTarget; }
    public NavMeshAgent Navigator => GetComponent<NavMeshAgent>();
    void OnEnable()
    {
        m_teleportCooldown = StartCoroutine(Cooldown());
        m_defaultTarget = transform.position;
    }
    protected IEnumerator TrackTarget()
    {
        while (m_target != null && Navigator.isActiveAndEnabled) 
        {
            // update current target destination
            m_precisePosition = m_target.transform.position;
            float directDist = Vector2.Distance(PlayerMovement.PlayerPosition, transform.position);
            // try teleport near player if too far from destimation
            if (Navigator.remainingDistance > m_warpDistanceThreshold && 
                directDist > m_warpDistanceThreshold)
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
}
