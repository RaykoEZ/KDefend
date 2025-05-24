using UnityEngine;
using UnityEngine.UI;
using Curry.Explore;

public class DeliveryHandle : HideableUI
{
    [SerializeField] Sprite m_deliTypeFood = default;
    [SerializeField] Sprite m_deliTypeIntel = default;
    [SerializeField] Sprite m_deliTypeSmuggle = default;
    [SerializeField] Image m_deliverType = default;

    [SerializeField] DirectionPointer m_pointer = default;
    [SerializeField] LocationHandler m_origins = default;
    [SerializeField] LocationHandler m_destinations = default;
    bool m_isActive = false;
    DeliveryDetail m_currentRef;
    public DeliveryDetail CurrentDelivery => m_currentRef;
    public bool IsActive { get => m_isActive; }
    public void InitDeliveryPickup(DeliveryDetail obj)
    {
        m_isActive = true;
        m_currentRef = obj;
        Sprite deliType;
        // Setup visual
        switch (m_currentRef.DeliverType)
        {
            case DeliveryType.Food:
                deliType = m_deliTypeFood;
                break;
            case DeliveryType.Intel:
                deliType = m_deliTypeIntel;
                break;
            case DeliveryType.Smuggle:
                deliType = m_deliTypeSmuggle;
                break;
            default:
                deliType = m_deliTypeFood;
                break;
        }
        m_deliverType.sprite = deliType;
        SetSize(0);
        var loc = m_origins.GetLocation(obj.OriginIndex);
        PointTo(loc);
        Show();
    }
    // Call this after player gets delivery package, begin delivery
    public void BeginDelivery() 
    {
        var loc = m_destinations.GetLocation(m_currentRef.OriginIndex);
        PointTo(loc);
    }
    public void ResetHandle()
    {
        m_isActive = false;
        SetSize(0);
        StopPointing();
        // reset to empty
        m_currentRef = new DeliveryDetail { };
        Hide();
    }
    public void SetSize(int size = 0)
    {
        var anim = GetAnim;
        anim?.SetInteger("size", size);
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
