using UnityEngine;
// Use this to spawn impact effect under this parent
public class HitImpactHandler : MonoBehaviour 
{
    [SerializeField] AttackImpact m_impactAssetRef = default;
    public void SpawnImpactAt(BaseEntity target)
    {
        AttackImpact ret = GameUtil.SpawnObject(m_impactAssetRef, localposition: Vector3.zero, target.transform.root);
        // instantiate impact at the contact point of the attack
        ret.transform.position = target.CurrentContactPoint;
        ret?.TriggerAtWorldPosition();
    }
}
