using UnityEngine;
using UnityEngine.UI;
using Curry.Explore;
public class DeliveryIcon : HideableUI
{
    [SerializeField] Sprite m_deliTypeFood = default;
    [SerializeField] Sprite m_deliTypeIntel = default;
    [SerializeField] Sprite m_deliTypeSmuggle = default;

    [SerializeField] Image m_deliverType = default;
    DeliveryDetail m_currentRef;
    public DeliveryDetail CurrentDelivery => m_currentRef;
    public void StartIcon(DeliveryDetail obj)
    {
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
        Show();
    }
    public void ResetIcon()
    {
        SetSize(0);
        // reset to empty
        m_currentRef = new DeliveryDetail { };
        Hide();
    }
    public void SetSize(int size = 0) 
    {
        var anim = GetAnim;
        anim?.SetInteger("size", size);
    }
}
