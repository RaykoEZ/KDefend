using System.Collections.Generic;
using UnityEngine;
// Items that stays in inventory, with trigger effects
public class Collectible : Item 
{
    [SerializeField] List<KDEvent> m_effectTriggers = default;
    public void Init(Player user) 
    { 
        m_user = user;
        foreach (var trigger in m_effectTriggers) 
        {
            trigger?.InitGlobalListeners();
        }
    }
    public override void OnPickup()
    {
        GetComponent<Collider2D>().enabled = false;
        m_onPickup?.Invoke(m_user);
        m_pickUpCommand?.Disable();
    }
}