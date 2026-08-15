using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
[Serializable]
public enum CompareMode
{
    Equal = 0,
    LessThan = 1,
    GreaterThan = 2,
    LessOrEqual = 3,
    GreaterOrEqual = 4,
}
// check entity hp state when healed or taking damage, trigger scene event on threshold
public class HealthThresholdListener : MonoBehaviour 
{
    [SerializeField] bool m_activateOnEnable = default;
    [SerializeField] CompareMode m_compareMode = default;
    [SerializeField, Range(0f, 1f)] float m_triggerHealthThreshold = default;
    [SerializeField] BaseEntity m_target = default;
    [SerializeField] UnityEvent<BaseEntity> m_onHpThresholdReached = default;
    Coroutine m_temp;
    void OnEnable()
    {
        if (m_activateOnEnable) 
        {
            Activate();
        }
    }
    public void Activate()
    {
        StopAllCoroutines();
        m_target.OnHeal += OnUpdate;
        m_target.OnTakeDamage += OnUpdate;
    }
    public void Deactivate()
    {
        StopAllCoroutines();
        m_target.OnHeal -= OnUpdate;
        m_target.OnTakeDamage -= OnUpdate;
    }
    public void TemporaryDeactivate(float duration) 
    {
        StopAllCoroutines();
        m_temp = StartCoroutine(TemporaryDeactivate_Internal(duration));
    }
    IEnumerator TemporaryDeactivate_Internal(float duration) 
    {
        m_target.OnHeal -= OnUpdate;
        m_target.OnTakeDamage -= OnUpdate;
        yield return new WaitForSeconds(duration);
        m_target.OnHeal += OnUpdate;
        m_target.OnTakeDamage += OnUpdate;
        m_temp = null;
    }
    void OnUpdate(int _) 
    {
        int baseHp = Mathf.Max(m_target.BaseStats.Property.Health, 1);
        float current = (float)m_target.CurrentStats.Property.Health / (float)baseHp;
        bool trigger;
        switch (m_compareMode)
        {
            case CompareMode.Equal:
                trigger = Mathf.Approximately(current, m_triggerHealthThreshold);
                break;
            case CompareMode.LessThan:
                trigger = current < m_triggerHealthThreshold;
                break;
            case CompareMode.GreaterThan:
                trigger = current > m_triggerHealthThreshold;
                break;
            case CompareMode.LessOrEqual:
                trigger = current <= m_triggerHealthThreshold;
                break;
            case CompareMode.GreaterOrEqual:
                trigger = current >= m_triggerHealthThreshold;
                break;
            default:
                trigger = false;
                break;
        }
        if (trigger) 
        {
            m_onHpThresholdReached?.Invoke(m_target); 
        }
    }
}