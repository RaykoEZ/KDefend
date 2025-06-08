using System.Collections;
using UnityEngine;
public delegate void OnRoutineUpdate();
// sets one coroutine to repeat with time interval
public class RoutineCaller : MonoBehaviour 
{
    float m_timeInterval = 1f;
    bool m_inProgress = false;
    public float TimeInterval { get => m_timeInterval; set => m_timeInterval = value; }
    public event OnRoutineUpdate OnNewInterval;
    IEnumerator Routine_Internal(IEnumerator routine) 
    {
        while (m_inProgress)
        {
            // Notify for each interval if changes are needed
            OnNewInterval?.Invoke();
            yield return new WaitForSeconds(TimeInterval);
            yield return StartCoroutine(routine);
        }
    }
    public void StartRoutine(IEnumerator routine)
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