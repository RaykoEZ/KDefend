using System;
using UnityEngine;
using UnityEngine.Events;

public class WeekDayNpcUpdater : MonoBehaviour
{
    [SerializeField] UnityEvent m_onMonday = default;
    [SerializeField] UnityEvent m_onTuesday = default;
    [SerializeField] UnityEvent m_onWednesday = default;
    [SerializeField] UnityEvent m_onThursday = default;
    [SerializeField] UnityEvent m_onFriday = default;
    [SerializeField] UnityEvent m_onSaturday = default;
    [SerializeField] UnityEvent m_onSundayy = default;
    public void OnDayBegin(DayOfWeek dayOfWeek) 
    {
        switch (dayOfWeek)
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
                m_onSaturday?.Invoke();

                break;
            case DayOfWeek.Sunday:
                m_onSundayy?.Invoke();
                break;

            default:
                break;
        }
    }
}