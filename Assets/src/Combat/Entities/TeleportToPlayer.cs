using UnityEngine;
using UnityEngine.Events;

public class TeleportToPlayer: MonoBehaviour 
{
    [SerializeField] Player m_player = default;
    [SerializeField] UnityEvent m_onTeleport = default;
    public void TeleportTarget(GameObject target) 
    {
        target.transform.position = m_player.transform.position;
        m_onTeleport?.Invoke();
    }
}
