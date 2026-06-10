using UnityEngine;
// Use this to spawn impact effect under this parent
public class HitImpactHandler : MonoBehaviour 
{
    [SerializeField] AttackImpact m_impactAssetRef = default;
    [SerializeField] LayerMask m_detectHitFrom = default;
    public void SpawnImpactAt(BaseEntity target)
    {
        AttackImpact ret = GameUtil.SpawnObject(m_impactAssetRef, localposition: Vector3.zero, target.transform.root);
        // instantiate impact at the contact point of the attack
        ret.transform.position = GetContactPoint(transform.position, target.transform.position);
        ret?.TriggerAtWorldPosition();
    }
    // Do ths to find the position to render impact FX
    Vector3 GetContactPoint(Vector3 origin, Vector3 dest) 
    {
        var hit = Physics2D.Linecast(origin, dest, m_detectHitFrom);
        return hit? hit.point : dest;
    }
}
