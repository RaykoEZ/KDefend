using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Detects target entity in sight
[RequireComponent(typeof(Collider2D))]
public class EnemyTargetFinder : MonoBehaviour
{
    [SerializeField] float m_attackTimeInterval = default;
    [SerializeField] UnityEvent<BaseEntity> m_targetSighted = default;
    [SerializeField] UnityEvent<BaseEntity> m_targetLost = default;
    HashSet<BaseEntity> m_targetToSeek = new HashSet<BaseEntity>();
    BaseEntity m_baseInSight;
    public BaseEntity BaseInSight { get => m_baseInSight; }
    public HashSet<BaseEntity> TargetToSeek { 
        get => m_targetToSeek; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_targetToSeek == null) return;
        if (collision.gameObject.TryGetComponent(out BaseEntity entering) &&
            m_targetToSeek.Contains(entering))
        {
            m_baseInSight = entering;
            m_targetSighted?.Invoke(entering);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BaseEntity exiting) &&
            BaseInSight == exiting) 
        {
            m_baseInSight = null;
            m_targetLost?.Invoke(exiting);
        }
    }
}