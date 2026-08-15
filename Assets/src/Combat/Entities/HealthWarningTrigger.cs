using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class HealthWarningTrigger : MonoBehaviour 
{
    [Range(0.01f, 0.9f)]
    [SerializeField] float m_healthThreshold = default;
    [SerializeField] UnityEvent m_onLowHealth = default;
    [SerializeField] Slider m_hpBar = default;
    public void OnHealthChange() 
    { 
        float ratio = m_hpBar.value / m_hpBar.maxValue;
        // trigger low HP event is threshold reached on taking damage
        if (ratio < m_healthThreshold) 
        {
            m_onLowHealth?.Invoke();
        }
    }
}