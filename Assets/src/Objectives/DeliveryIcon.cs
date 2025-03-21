using UnityEngine;
using UnityEngine.UI;
using Curry.Explore;
public class DeliveryIcon : HideableUI
{
    [SerializeField] Image m_deliverType = default;
    [SerializeField] Image m_rank = default;
    DeliveryObjective m_currentRef;
    public event OnTimeOut<DeliveryIcon> TimerOut;
    public DeliveryObjective CurrentDelivery => m_currentRef;
    public void SetupIcon(DeliveryObjective obj)
    {
        m_currentRef = obj;
        // Setup visual
    }
    public void ResetIcon()
    {
        SetSize(0);
        // reset to empty
        m_currentRef = null;
    }
    public void SetSize(int size = 0) 
    {
        var anim = GetAnim;
        anim?.SetInteger("size", size);
    }
}
