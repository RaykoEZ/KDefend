using UnityEngine;

public class DeliverySpawner : MonoBehaviour 
{
    [SerializeField] Transform m_parent = default;
    [SerializeField] DeliveryBox m_boxRef = default;
    [SerializeField] LocationHandler m_origins = default;
    public DeliveryBox SpawnDeliveryBox(DeliveryDetail detail)
    {
        var loc = m_origins.GetLocation(detail.OriginIndex);
        DeliveryBox ret = GameUtil.SpawnObject(m_boxRef, loc.position, m_parent);
        ret?.Init(detail);
        ret.gameObject.SetActive(true);
        return ret;
    }
}
