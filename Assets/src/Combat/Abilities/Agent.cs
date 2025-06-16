using System.Collections.Generic;
using UnityEngine;
//enemy agent behaviour
public class Agent : AbilityHandler
{
    [SerializeField] Stealth m_stealth = default;
    [SerializeField] Reinforcement m_callHelp = default;
    [SerializeField] List<BaseWeapon> m_onLowHealth = default;
    public override List<ActiveAbility> Abilities => new List<ActiveAbility> { m_stealth, m_callHelp };
    public override void OnTakeHit() 
    {
        base.OnTakeHit();
        if (Self.HpRatio < 0.5f)
        {
            OnLowHp(true);
        }
    }
    protected override void StartupEffects()
    {
        if (Self.HpRatio < 0.5f)
        {
            OnLowHp(reinforcement: false);
        }
    }
    // when low on Hp, hide and call for help
    void OnLowHp(bool reinforcement = false) 
    {
        Stealth();
        Self.SetWeapons(m_onLowHealth);
        if (reinforcement)
        {
            Reinforce();
        }
    }
    // when < 50% HP, activate stealth & calls help
    void Stealth() 
    {
        m_stealth?.TryUse();
    }
    // call help
    void Reinforce() 
    {
        m_callHelp?.TryUse();
    }
}
