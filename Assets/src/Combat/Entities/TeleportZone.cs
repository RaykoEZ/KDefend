using UnityEngine;
using UnityEngine.Events;
// Teleports player to a predefined position and trigger events
public class TeleportZone : MonoBehaviour
{
    [SerializeField] bool m_enableOnInit = default;
    [SerializeField] Transform m_destination = default;
    [SerializeField] UnityEvent m_onEnable = default;
    [SerializeField] UnityEvent m_onDisable = default;
    [SerializeField] UnityEvent m_onTeleport = default;
    bool m_enabled = false;
    void OnEnable()
    {
        if (m_enableOnInit) 
        {
            m_enabled = true;
            m_onEnable?.Invoke();
        }
    }
    public void Enable() 
    {
        m_enabled = true;
        m_onEnable?.Invoke();
    }
    public void Disable() 
    {
        m_enabled = false;
        m_onDisable?.Invoke();
    }
    public void TryTeleport(Collider2D target) 
    { 
        if (!m_enabled || target == null || target.attachedRigidbody == null) return;
        if (target.attachedRigidbody.TryGetComponent(out Player player)) 
        {
            player.transform.position = m_destination.transform.position;
            m_onTeleport?.Invoke();
        }
    }
}