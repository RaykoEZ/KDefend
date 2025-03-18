using System.Collections.Generic;
using UnityEngine;

public delegate void OnTimeOut<T>(T sender);
public class DeliveryIcon : MonoBehaviour 
{
    int m_currentTimer = 1;
    DeliveryObjective m_currentRef;
    public event OnTimeOut<DeliveryIcon> TimerOut;
    public int CurrentTimer { get => m_currentTimer; }
    public DeliveryObjective CurrentDelivery => m_currentRef;
    public virtual void UpdateTimer(KDefenderEventContext _) 
    {
        // counting down
        m_currentTimer--;
        if (m_currentTimer == 0)
        {
            TimerOut?.Invoke(this);
        }
    }
}
public class DeliveryPrompter : MonoBehaviour 
{
    [SerializeField] List<DeliveryIcon> m_currentIcons = default;
    public void NewDelivery(DeliveryObjective newDelivery) 
    { 
    }
    public void OnDeliverySuccess() 
    { 

    }
    public void OnDeliveryFail() 
    { 
    
    }
}
