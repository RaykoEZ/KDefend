using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Executive : AbilityHandler
{
    [SerializeField] Deadeye m_railCannon = default;
    [SerializeField] WaveStrike m_waveStrike = default;
    [SerializeField] RangeDetector m_range = default;
    public override List<ActiveAbility> Abilities => new List<ActiveAbility> { m_railCannon, m_waveStrike };
    // interrupt channeling when taking a hit
    void FixedUpdate()
    {
        if (m_range.IsPlayerInRange())
        {
            m_waveStrike?.TryUse();
        }
    }
    public override void OnTakeHit()
    {
        base.OnTakeHit();
        UpdateStrikeFrequency();
    }
    void UpdateStrikeFrequency() 
    {
        float hpRatio = Self.HpRatio;
        // HP threshold to increase strike frequency
        if (hpRatio < 0.75f && hpRatio > 0.5f)
        {
            m_waveStrike.StrikeTimeInterval = 1.5f;
        }
        else if (hpRatio <= 5f && hpRatio > 0.25f)
        {
            m_waveStrike.StrikeTimeInterval = 0.75f;
        }
    }
}
