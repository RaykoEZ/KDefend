using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct ThreatScalings 
{
    public float RewardMultiplier;
    public float TimeBetweenRoutineWave;
    public static ThreatScalings Default => new ThreatScalings { RewardMultiplier = 1.0f, TimeBetweenRoutineWave = 60f };
}
public class ThreatHandler : MonoBehaviour 
{
    // threat level is the index of this list
    [SerializeField] List<ThreatScalings> m_threatMultipliers = default;
    // current wave number
    int m_currentThreatStage = 0;
    public int CurrentThreat => m_currentThreatStage;
    public ThreatScalings GetCurrentThreatMultiplier()
    {
        if (m_threatMultipliers.Count == 0 || m_currentThreatStage >= m_threatMultipliers.Count) 
            return ThreatScalings.Default;
        return m_threatMultipliers[m_currentThreatStage];
    }
    public void SetThreat(int threat) 
    {
        m_currentThreatStage = threat;
    }
    // increase threat level
    public void IncreaseThreat()
    {
        // Increment to spawn next wave
        m_currentThreatStage++;
    }
    public void DecreseThreat()
    {
        // Increment to spawn next wave
        m_currentThreatStage = Mathf.Max(m_currentThreatStage - 1, 0);
    }
}
