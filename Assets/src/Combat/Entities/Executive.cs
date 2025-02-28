using System;
using System.Collections.Generic;
using UnityEngine;
// Script for Executive enemy behaviours
public class Executive : MonoBehaviour 
{
    [SerializeField] EnemyWaveManager m_waveSpawner = default;
    public virtual void Reinforcement() 
    {
        m_waveSpawner?.StartWave();
    }
}