using UnityEngine;
using UnityEngine.AI;
// attach on npc object for enemies to flank/enter formation in pursuit
public class FormationHandler : MonoBehaviour
{
    /// <summary>
    /// Angle range to assign formation position around the chase target. Range: +/- 90 degrees.
    /// </summary>
    [Range(0f, 90f)]
    [SerializeField] float m_encircleAngleRange = default;
    [Range(10f, 2000f)]
    [SerializeField] float m_predictionDistance = default;
    // current formation angle
    float m_formationAngle = 0f;
    bool m_randomizeNextPositon = false;
    public bool RandomizeFormationPosition 
    { get => m_randomizeNextPositon; set => m_randomizeNextPositon = value; }
    void OnEnable()
    {
        RandomizeFormationAngle();
    }
    // return a destination for npc movement formation to chase after target, teleport to this position
    public bool TryGetFormationPosition(BaseEntity chaseTarget, out Vector3 warpPosition)
    {
        if (m_randomizeNextPositon) 
        {
            RandomizeFormationAngle();
        }
        // get a copy of current player moving direction
        Vector3 playerDir = PlayerMovement.PlayerMovementDirection == Vector2.zero? Vector2.up : PlayerMovement.PlayerMovementDirection;
        // get randomized rotated direction from player's movement direction
        Vector3 angledDir = Quaternion.AngleAxis(m_formationAngle, Vector3.forward) * playerDir * m_predictionDistance;
        Vector3 dest = chaseTarget.transform.position + angledDir;
        // find suitable position to set as destination
        var areaMaskFromName = 1 << NavMesh.GetAreaFromName("Walkable");
        if (NavMesh.SamplePosition(dest, out NavMeshHit hit, 100f, areaMaskFromName))
        {
            warpPosition = hit.position;
            return true;
        }
        else 
        {
            warpPosition = transform.position;
            return false;
        }
    }
    // set a new random angle of the formation 
    void RandomizeFormationAngle() 
    {
        m_formationAngle = Random.Range(-m_encircleAngleRange, m_encircleAngleRange);
    }
}
