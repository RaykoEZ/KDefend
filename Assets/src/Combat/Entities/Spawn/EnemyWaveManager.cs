using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnWaveStart(int waveNumber);
public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] List<EnemyWaveDetail> m_waves = default;
    [SerializeField] List<EnemySpawner> m_spawners = default;
    [SerializeField] List<BaseEntity> m_targetPriorityList = default;
    [SerializeField] BaseEntity m_defaultAggroTarget = default;
    [SerializeField] CoroutineManager m_waveSpawn = default;
    bool m_waveInProgress = false;
    public event OnWaveStart OnStart;
    // current wave number
    int m_currentWave = 0;
    public int CurrentWave => m_currentWave;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OnStartGame());
    }
    IEnumerator OnStartGame() 
    {
        yield return new WaitForSeconds(0.5f);
        // stop bgm until we start next wave
        StartWave();
        yield return new WaitForSeconds(0.8f);
    }
    public void StartWave() 
    {
        if (m_waveInProgress) return;

        m_waveInProgress = true;
        m_waveSpawn.ScheduleCoroutine(Spawn_Internal(), true);
    }
    public void PauseWave() 
    {
        if (!m_waveInProgress) return;
        m_waveSpawn.StopCurrentCoroutine();
        m_waveInProgress = false;
    }
    public virtual void Spawn(EnemyWaveDetail newWave) 
    {
        // random spawners
        List<EnemySpawner> spawners = SamplingUtil.SampleFromList(
                m_spawners, newWave.GroupsToSpawn.Count, uniqueResults: true);
        int i = 0;
        foreach (var group in newWave.GroupsToSpawn)
        {
            // spawn the group
            spawners[i].Spawn(group.SpawnRef, group.NumToSpawn, 0.1f, InitEnenmy);
            i++;
        }
    }
    IEnumerator Spawn_Internal() 
    {
        while (m_currentWave < m_waves.Count) 
        {
            EnemyWaveDetail wave = m_waves[m_currentWave];
            OnStart?.Invoke(m_currentWave + 1);
            Spawn(wave);
            // after spawning, wait for a set duration
            yield return new WaitForSeconds(wave.SecondsBeforeNextWave);
            // Increment to spawn next wave
            m_currentWave++;
        }
        m_waveInProgress = false;
    }
    void InitEnenmy(Enemy spawned) 
    {
        spawned.Init(m_targetPriorityList, m_defaultAggroTarget);
    }
}
