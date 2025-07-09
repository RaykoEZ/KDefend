using System;
using System.Collections.Generic;
using UnityEngine;
// Script for Executive enemy abilities
public class Executive : AbilityHandler
{
    public override List<ActiveAbility> Abilities => throw new NotImplementedException();
    // a charged beam attack, cannot use Thousand star while charging
    // Use Deadeye to aim and release Beam
    void RailCannon() { }
    // multiple spray attack
    void ThousandStar() { }

    // Summon guards to box player in
    void SummomArena() { }
}
// Summon Drone around user
public class ThousandStar : ActiveAbility
{
    [SerializeField] int m_numToSummon = default;
    [SerializeField] float m_rotationSpeed = default;
    [SerializeField] BaseEntity m_toSpawn = default;
    protected override void Effect_Internal()
    {
        throw new NotImplementedException();
    }
}
// Summon guards to surround the player
public class SummonArena : ActiveAbility
{
    [SerializeField] Enemy m_arenaSpawnRef = default;
    [SerializeField] BoxSpawner m_spawner = default;
    protected override void Effect_Internal()
    {
        m_spawner?.Spawn(m_arenaSpawnRef, transform.parent, 0.1f);
    }
}