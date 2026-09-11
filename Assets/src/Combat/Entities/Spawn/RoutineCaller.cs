using System.Collections;
using System;
using UnityEngine;
public delegate void OnRoutineUpdate();
// sets one coroutine to repeat with time interval
public class RoutineCaller : MonoBehaviour 
{
    [Range(1, 9999)]
    [SerializeField] float m_timeInterval = default;
    float m_defaultTimeInterval = 0f;
    bool m_inProgress = false;
    public float TimeInterval { get => m_timeInterval; set => m_timeInterval = value; }
    public event OnRoutineUpdate OnNewInterval;
    void Awake()
    {
        m_defaultTimeInterval = m_timeInterval;
    }
    public void ResetTimeInterval() 
    {
        TimeInterval = m_defaultTimeInterval;
    }
    IEnumerator Routine_Internal(Action routine) 
    {
        while (m_inProgress)
        {
            // Notify for each interval if changes are needed
            OnNewInterval?.Invoke();
            yield return new WaitForSeconds(TimeInterval);
            routine?.Invoke();
        }
    }
    public void StartRoutine(Action routine)
    {
        if (m_inProgress) return;
        m_inProgress = true;
        StartCoroutine(Routine_Internal(routine));
    }
    public void StopRoutine()
    {
        StopAllCoroutines();
        m_inProgress = false;
    }
}