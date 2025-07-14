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
    List<BaseEntity> m_targetsInView = new List<BaseEntity>();
    void Start()
    {
    }
    public bool IsInRange(BaseEntity target) 
    {
        return m_targetsInView.Contains(target);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null) return;
        bool check = (collision.attachedRigidbody.TryGetComponent(out Player entering));
        if (m_detectAnyEntity || check)
        {
            m_targetsInView.Add(entering);
            m_targetSighted?.Invoke(entering);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null) return;
        if (collision.attachedRigidbody.TryGetComponent(out Player exiting)) 
        {
            m_targetLost?.Invoke();
            m_targetsInView.Remove(exiting);
        }
    }
}
