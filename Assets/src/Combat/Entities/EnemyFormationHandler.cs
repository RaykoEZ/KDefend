using UnityEngine;
using UnityEngine.AI;
// attach on npc object for enemies to flank/enter formation in pursuit
public class EnemyFormationHandler : MonoBehaviour
{
    /// <summary>
    /// Angle range to assign formation position around the chase target. Range: +/- 180 degrees.
    /// </summary>
    [Range(0f, 180f)]
    [SerializeField] float m_encircleAngleRange = default;
    /// <summary>
    /// When chaser is close enough to target, direct to target position instead. 
    /// This trigger theshold scales with Target Distance Radius 
    /// </summary>
    [Range(0.1f, 1f)]
    [SerializeField] float m_targetLockOnThesholdDistance = default;
    // current formation angle
    float m_formationAngle = -360f;
    bool m_randomizeNextPositon = false;
    public bool RandomizeFormationPosition 
    { get => m_randomizeNextPositon; set => m_randomizeNextPositon = value; }
    // return a destination for npc movement formation to chase after target
    public Vector2 GetFormationPosition(BaseEntity chaseTarget)
    {
        if (m_randomizeNextPositon) 
        {
            RandomizeFormationAngle();
        }
        // get a copy of current player moving direction
        Vector3 playerDir = PlayerMovement.PlayerMovementDirection;
        // get randomized rotated direction from player's movement direction
        Vector3 angledDir = Quaternion.AngleAxis(m_formationAngle, Vector3.forward) * playerDir;
        // get distance mod
        angledDir *= GetFormationRadius(chaseTarget);
        Vector3 dest = chaseTarget.transform.position + angledDir;
        // find suitable position to set as destination
        if (NavMesh.SamplePosition(dest, out NavMeshHit hit, 50f, NavMesh.GetAreaFromName("Walkable")))
        {
            return hit.position;
        }
        else 
        {
            return chaseTarget.transform.position;
        }
    }
    protected float GetFormationRadius(BaseEntity chaseTarget) 
    {
        float dist = Vector3.Distance(chaseTarget.transform.position, transform.position);
        // if distance is too close, default to point-blank distance
        // decided distance scales with distance from the two objects
        return dist <= m_targetLockOnThesholdDistance ? 0f : dist * 0.5f;
    }
    // set a new random angle of the formation 
    void RandomizeFormationAngle() 
    {
        m_formationAngle = Random.Range(-m_encircleAngleRange, m_encircleAngleRange);
    }
}
