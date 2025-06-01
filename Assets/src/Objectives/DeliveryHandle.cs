using UnityEngine;
using UnityEngine.UI;
using Curry.Explore;

public class DeliveryHandle : MonoBehaviour
{
    [SerializeField] DirectionPointer m_pointer = default;
    [SerializeField] LocationHandler m_destinations = default;
    bool m_isActive = false;
    DeliveryDetail m_currentRef;
    public DeliveryDetail CurrentDelivery => m_currentRef;
    public bool IsActive { get => m_isActive; }
    public void InitDeliveryPickup(DeliveryDetail obj)
    {
        m_isActive = true;
        m_currentRef = obj;
    }
    // Call this after player gets delivery package, begin delivery
    public void BeginDelivery() 
    {
        Transform loc = m_destinations.GetLocation(m_currentRef.DestinationIndex);
        loc?.GetComponent<DeliveryDestination>()?.ExpectPackage(m_currentRef);
        PointTo(loc);
    }
    public void ResetHandle()
    {
        Transform loc = m_destinations.GetLocation(m_currentRef.DestinationIndex);
        m_isActive = false;
        StopPointing();
        // reset to empty
        m_currentRef = new DeliveryDetail { };
    }
    public void OnDeliverySuccess(DeliveryDetail obj)
    {
        // get rewards and animation here
        ResetHandle();
    }
    void PointTo(Transform newTarget) 
    {
        m_pointer.gameObject.SetActive(true);
        m_pointer.PointToward(newTarget);
    }
    void StopPointing()
    {
        m_pointer.StopPointing();
        m_pointer.gameObject.SetActive(false);
    }
}
