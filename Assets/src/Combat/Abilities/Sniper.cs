using System.Collections.Generic;
using UnityEngine;

public class Sniper : AbilityHandler
{
    [SerializeField] Aiming m_aim = default;
    [SerializeField] SnipeShot m_snipe = default;
    public override List<ActiveAbility> Abilities => new List<ActiveAbility> { m_aim, m_snipe };
    void Aim() 
    {
        m_aim?.TryUse();
    }
    void Snipe() 
    {
        m_snipe?.TryUse();
    }
}
