using UnityEngine;
using System.Collections.Generic;

public class Reinforcement : ActiveAbility
{
    [SerializeField] SpawnWave m_reinforcement = default;
    // require time to charge
    [SerializeField] int m_channellingTime = default;
    bool m_inProgress = false;
    protected override void Effect_Internal()
    {
        throw new System.NotImplementedException();
    }
}
