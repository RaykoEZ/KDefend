using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ColliderEventTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent<Collider> m_onEnter = default;
    [SerializeField] UnityEvent<Collider> m_onExit = default;
    void OnTriggerEnter(Collider collision)
    {
        m_onEnter?.Invoke(collision);
    }
    void OnTriggerExit(Collider other)
    {
        m_onExit?.Invoke(other);
    }
}