using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Items that stays in inventory, with trigger effects
public class Collectible : Item 
{
    [SerializeField] List<KDEvent> m_effectTriggers = default;
    // apply additional effects depending on item level
    [SerializeField] List<UnityEvent<Player>> m_levelEffects = default;
    public void Init(Player user) 
    { 
        m_user = user;
        foreach (var trigger in m_effectTriggers) 
        {
            trigger?.InitGlobalListeners();
        }
    }
    // on pickup, activate effect but stays in inventory
    public override void OnPickup()
    {
        GetComponent<Collider2D>().enabled = false;
        m_onPickup?.Invoke(m_user);
        m_user?.Inventory?.ObtainItem(this);
        m_pickUpCommand?.Disable();
    }
}