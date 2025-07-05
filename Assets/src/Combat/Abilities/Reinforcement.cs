using UnityEngine;
using System.Collections.Generic;
using Curry.Events;
public class Reinforcement : ActiveAbility
{
    [SerializeField] SpawnWave m_reinforcement = default;
    [SerializeField] CurryGameEventTrigger m_spawnHelp = default;
    // require time to charge
    [SerializeField] int m_channellingTime = default;
    protected override void Effect_Internal()
    {
        if (m_channeling != null) return;
        StartChanneling(m_channellingTime, SpawnHelp);
    }
    void SpawnHelp() 
    {
        var payload = new Dictionary<string, object> { {"wave", m_reinforcement } };
        EventInfo info = new EventInfo(payload);
        m_spawnHelp?.TriggerEvent(info);
    }
}
