using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Detects target entity in sight
[RequireComponent(typeof(Collider2D))]
public class EnemyTargetFinder : MonoBehaviour
{
    [SerializeField] UnityEvent<BaseEntity> m_targetSighted = default;
    [SerializeField] UnityEvent m_targetLost = default;
    int m_targetPriority = -1;
    List<BaseEntity> m_targetPriorityList = new List<BaseEntity>();
    List<BaseEntity> m_targetsInView = new List<BaseEntity>();
    public List<BaseEntity> TargetsInSight { get => m_targetsInView; }
    public void AddTargets(List<BaseEntity> interests)
    {
        m_targetPriorityList.AddRange(interests);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_targetPriorityList == null) return;
        if (collision.attachedRigidbody.TryGetComponent(out BaseEntity entering) &&
            m_targetPriorityList.Contains(entering))
        {
            m_targetsInView.Add(entering);
            // send priority value
            int p = m_targetPriorityList.IndexOf(entering);
            if (p > m_targetPriority)
            {
                m_targetPriority = p;
                m_targetSighted?.Invoke(entering);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.TryGetComponent(out BaseEntity exiting)) 
        {
            int p = m_targetPriorityList.IndexOf(exiting);
            if (p == m_targetPriority) 
            {
                m_targetPriority = -1;
                m_targetLost?.Invoke();
            }
            m_targetsInView.Remove(exiting);
        }
    }
}