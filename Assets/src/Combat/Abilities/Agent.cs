using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//enemy agent behaviour
public class Agent : AbilityHandler
{
    [SerializeField] Stealth m_stealth = default;
    [SerializeField] Reinforcement m_callHelp = default;
    [SerializeField] AttackHandler m_attack = default;
    [SerializeField] UnityEvent m_onLowHealth = default;
    public override List<ActiveAbility> Abilities => new List<ActiveAbility> { m_stealth, m_callHelp };
    public override void OnTakeHit() 
    {
        base.OnTakeHit();
        if (Self.HpRatio < 0.5f)
        {
            OnLowHp();
        }
    }
    protected override void StartupEffects()
    {
        if (Self.HpRatio < 0.5f)
        {
            OnLowHp();
        }
    }
    // when low on Hp, hide and call for help
    void OnLowHp() 
    {
        Stealth();
        m_onLowHealth?.Invoke();

    }
    // when < 50% HP, activate stealth & calls help
    public void Stealth() 
    {
        m_stealth?.TryUse();
    }
}
