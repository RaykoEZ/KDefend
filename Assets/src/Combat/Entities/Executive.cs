using System;
using System.Collections.Generic;
using UnityEngine;
// Script for Executive enemy abilities
public class Executive : AbilityHandler
{
    public override List<ActiveAbility> Abilities => throw new NotImplementedException();
    // a charged beam attack, cannot use Thousand star while charging
    void RailCannon() { }
    // multiple spray attack
    void ThousandStar() { }
    // rotating linked shields, protects from attacks
    // shield takes damage in each section, destroy shield if one section collapses
    void NanobotArray() { }
    // Summon guards to box player in
    void SummomArena() { }
}