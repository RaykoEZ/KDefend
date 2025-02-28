using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Detects target entity in sight
[RequireComponent(typeof(Collider2D))]
public class RangeDetector : MonoBehaviour
{
    [SerializeField] bool m_detectAnyEntity = default;
    [SerializeField] UnityEvent<BaseEntity> m_targetSighted = default;
    [SerializeField] UnityEvent m_targetLost = default;
    [SerializeField] List<BaseEntity> TEST_P_LIST = default;
    int m_targetPriority = -1;
    List<BaseEntity> m_targetPriorityList = new List<BaseEntity>();
    List<BaseEntity> m_targetsInView = new List<BaseEntity>();
    public IReadOnlyList<BaseEntity> TargetPriorityList { get => m_targetPriorityList;}
    void Start()
    {
        m_targetPriorityList.AddRange(TEST_P_LIST);
    }
    public void AddTargets(List<BaseEntity> interests)
    {
        m_targetPriorityList.AddRange(interests);
    }
    public bool IsInRange(BaseEntity target) 
    {
        return m_targetsInView.Contains(target);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_targetPriorityList == null) return;
        bool check = (collision.attachedRigidbody.TryGetComponent(out BaseEntity entering) &&
            m_targetPriorityList.Contains(entering));
        if (m_detectAnyEntity || check)
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
        if (collision.attachedRigidbody.TryGetComponent(out BaseEntity exiting) &&
            m_targetPriorityList.Contains(exiting)) 
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
