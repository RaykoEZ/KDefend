using System.Collections.Generic;
using UnityEngine;

public class Sniper : AbilityHandler
{
    [SerializeField] Deadeye m_aim = default;
    public override List<ActiveAbility> Abilities => new List<ActiveAbility> { m_aim };   
    public void Aim() 
    {
        m_aim?.TryUse();
    }
}
