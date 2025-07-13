using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Script for Executive enemy abilities
public class Executive : AbilityHandler
{
    [SerializeField] Deadeye m_cannon = default;
    [SerializeField] SummonArena m_summon = default;
    public override List<ActiveAbility> Abilities => new List<ActiveAbility> { m_cannon , m_summon};
    // a charged beam attack, cannot use Thousand star while charging
    // Use Deadeye to aim and release Beam
    void RailCannon() { }

    // Summon guards to box player in
    void SummomArena() { }
}
