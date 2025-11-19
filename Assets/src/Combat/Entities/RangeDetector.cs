using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Rendering.CameraUI;
public delegate void OnRangeUpdate(BaseEntity target);
// Detects target entity in sight
[RequireComponent(typeof(Collider2D))]
public class RangeDetector : MonoBehaviour
{
    [SerializeField] bool m_detectAnyEntity = default;
    [SerializeField] UnityEvent<BaseEntity> m_targetSighted = default;
    [SerializeField] UnityEvent m_targetLost = default;
    List<BaseEntity> m_targetsInView = new List<BaseEntity>();
    bool m_currentlyDetectAll = false;
    public event OnRangeUpdate OnEnter;
    public event OnRangeUpdate OnExit;
    public bool CurrentlyDetectAll { get => m_currentlyDetectAll; set => m_currentlyDetectAll = value; }
    public IReadOnlyList<BaseEntity> TargetsInView => m_targetsInView;

    void OnEnable()
    {
        m_currentlyDetectAll = m_detectAnyEntity;
    }
    public bool IsInRange(BaseEntity target) 
    {
        return target != null && m_targetsInView.Contains(target);
    }
    public bool IsPlayerInRange() 
    { 
        return m_targetsInView.Find((i) => i is Player);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null) return;
        // check for duplicate collision reporting and whether it's a player
        bool check = (collision.attachedRigidbody.TryGetComponent(out BaseEntity entering)) &&
            entering is Player;
        if ((m_currentlyDetectAll || check) && !IsInRange(entering))
        {
            m_targetsInView.Add(entering);
            m_targetSighted?.Invoke(entering);
            OnEnter?.Invoke(entering);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null) return;
        if (collision.attachedRigidbody.TryGetComponent(out BaseEntity exiting)) 
        {
            m_targetLost?.Invoke();
            m_targetsInView.Remove(exiting);
            OnExit?.Invoke(exiting);
        }
    }
}
