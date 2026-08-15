using System;
using UnityEngine;
using UnityEngine.Events;
// handle general/specific event calls when a new day begins
public class DayEventHandler : MonoBehaviour
{
    // general new day triggers
    [SerializeField] UnityEvent<DayOfWeek> m_onNewDay = default;
    [SerializeField] UnityEvent m_onMonday = default;
    [SerializeField] UnityEvent m_onTuesday = default;
    [SerializeField] UnityEvent m_onWednesday = default;
    [SerializeField] UnityEvent m_onThursday = default;
    [SerializeField] UnityEvent m_onFriday = default;
    public void OnNewDay(DayOfWeek day) 
    {
        m_onNewDay?.Invoke(day);
        switch (day)
        {
            case DayOfWeek.Monday:
                m_onMonday?.Invoke();
                break;
            case DayOfWeek.Tuesday:
                m_onTuesday?.Invoke();
                break;
            case DayOfWeek.Wednesday:
                m_onWednesday?.Invoke();
                break;
            case DayOfWeek.Thursday:
                m_onThursday?.Invoke();
                break;
            case DayOfWeek.Friday:
                m_onFriday?.Invoke();
                break;
            case DayOfWeek.Saturday:
                break;
            case DayOfWeek.Sunday:
                break;
            default:
                break;
        }
    }

}
