using System;
using UnityEngine;

public class Heal : EffectModule
{
    [SerializeField] protected bool m_healAboveMaxHealth = default;
    [SerializeField] protected int m_healAmount = default;
    public override void Activate(BaseEntity target)
    {
        base.Activate(target);
        int hpLost = target.BaseStats.Property.Health - target.CurrentStats.Property.Health;
        if (hpLost < 0) return;
        int heal = m_healAmount;
        // if we don't overheal, clamp heal value
        if (!m_healAboveMaxHealth) 
        {
            heal = Mathf.Clamp(m_healAmount, 0, hpLost + 1);
        }
        target?.Heal(heal);
    }
}