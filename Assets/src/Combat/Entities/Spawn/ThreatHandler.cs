using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] UnityEvent m_onThreatStageUpdate = default;
    // current wave number
    int m_currentThreatStage = 0;
    // If this value reaches above 100, increase threat stage
    int m_currentThreatValue = 0;
    public int CurrentThreat => m_currentThreatStage;
    public ThreatScalings GetCurrentThreatMultiplier()
    {
        if (m_threatMultipliers.Count == 0 || m_currentThreatStage >= m_threatMultipliers.Count) 
            return ThreatScalings.Default;
        return m_threatMultipliers[m_currentThreatStage];
    }
    public void SetThreatStage(int threat) 
    {
        m_currentThreatStage = threat;
    }
    // increase threat level
    public void UpdateThreat(int increase)
    {
        m_currentThreatValue += increase;
        // Increment stage if threat exceeds 100
        if (m_currentThreatValue >= 100) 
        {
            m_currentThreatStage++;
            m_currentThreatValue = 0;
            m_onThreatStageUpdate?.Invoke();
        }
    }
}