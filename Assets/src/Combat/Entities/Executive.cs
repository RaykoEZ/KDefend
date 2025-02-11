using System;
using System.Collections.Generic;
using UnityEngine;
// Script for Executive enemy behaviours
public class Executive : MonoBehaviour 
{
    [Range(1f, 1024f)]
    [SerializeField] float m_retreatRadius = default;
    [SerializeField] EnemyWaveManager m_waveSpawner = default;
    // Clear nearby enemy upon defeat
    public virtual void SoundRetreat() 
    {
        m_waveSpawner?.PauseWave();
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, m_retreatRadius, Vector2.zero);
        foreach (var item in hits)
        {
            if (item.transform.TryGetComponent(out Enemy result))
            {
                // destroy for now
                Destroy(result);
            }
        }
    }
    public virtual void Reinforcement() 
    {
        m_waveSpawner?.StartWave();
    }
}