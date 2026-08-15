using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ColliderEventTrigger2D : MonoBehaviour
{
    [SerializeField] UnityEvent<Collider2D> m_onEnter = default;
    [SerializeField] UnityEvent<Collider2D> m_onExit = default;
    void OnTriggerEnter2D(Collider2D collision)
    {
        m_onEnter?.Invoke(collision);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        m_onExit?.Invoke(other);
    }
}
