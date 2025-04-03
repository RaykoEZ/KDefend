using UnityEngine;
using UnityEngine.UI;
using Curry.Explore;
public class DeliveryIcon : HideableUI
{
    [SerializeField] Sprite m_deliTypeFood = default;
    [SerializeField] Sprite m_deliTypeIntel = default;
    [SerializeField] Sprite m_deliTypeSmuggle = default;

    [SerializeField] Image m_deliverType = default;
    DeliveryObjective m_currentRef;
    public DeliveryObjective CurrentDelivery => m_currentRef;
    public void StartIcon(DeliveryObjective obj)
    {
        m_currentRef = obj;
        Sprite deliType;
        // Setup visual
        switch (m_currentRef.Detail.DeliverType)
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
        m_currentRef = null;
        Hide();
    }
    public void SetSize(int size = 0) 
    {
        var anim = GetAnim;
        anim?.SetInteger("size", size);
    }
}
