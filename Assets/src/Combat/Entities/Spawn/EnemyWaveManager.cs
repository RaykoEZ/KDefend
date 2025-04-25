using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public delegate void OnWaveStart(int waveNumber);
public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] ThreatHandler m_threeat = default;
    [SerializeField] EnemyAssetList m_enemyRefs = default;
    // waves triggered by game event
    [SerializeField] List<SpawnWave> m_staticSpawns = default;
    // frequent spawns bound by timer, depending on threat level and game state trigger
    [SerializeField] List<SpawnWave> m_routineSpawnStages = default;
    // Spawner in locations
    [SerializeField] List<EnemySpawner> m_spawners = default;
    [SerializeField] UnityEvent<Enemy> m_onEnemySpawn = default;
    [SerializeField] UnityEvent<Enemy> m_onEnemyDefeat= default;
    [SerializeField] BaseEntity m_defaultAggroTarget = default;
    [SerializeField] CoroutineManager m_waveSpawn = default;
    bool m_waveInProgress = false;
    public event OnWaveStart OnStart;
    // current wave number
    int m_currentThreatStage = 0;
    public int CurrentWave => m_currentThreatStage;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        StartCoroutine(OnStartGame());
#endif   
    }
#if UNITY_EDITOR
    IEnumerator OnStartGame() 
    {
        yield return new WaitForSeconds(0.5f);
        // stop bgm until we start next wave
        RoutineWave();
        yield return new WaitForSeconds(0.8f);
    }
#endif
    public void RoutineWave() 
    {
        if (m_waveInProgress) return;

        m_waveInProgress = true;
        //make coroutine handle regular spawns
        m_waveSpawn.ScheduleCoroutine(RoutineSpawn_Internal(), true);
    }
    // trigger static spawn with scene/script events
    public void StaticSpawn(int spawnIndex) 
    { 
        if (spawnIndex > 0 && spawnIndex < m_staticSpawns.Count) 
        {
            SpawnWave(m_staticSpawns[spawnIndex]);
        }
    }
    public virtual void SpawnGroup(SpawnGroup newGroup) 
    {
        int i = newGroup.SpawnLocationIndex > 0 &&
                newGroup.SpawnLocationIndex < m_spawners.Count ?
                newGroup.SpawnLocationIndex : 0;
        foreach (var spawn in newGroup.GroupsToSpawn.Items)
        {
            var enemyRef = m_enemyRefs.GetEnemyRef(spawn.SpawnRef);
            // spawn the group
            m_spawners[i].Spawn(enemyRef, spawn.NumToSpawn, 0.1f, PrepareEnenmy);
        }
    }
    protected void SpawnWave(SpawnWave wave) 
    {
        foreach (var group in wave.EnemyGroups)
        {
            SpawnGroup(group);
        }
    }
    public void StopRoutineWave()
    {
        if (!m_waveInProgress) return;
        m_waveSpawn.StopCurrentCoroutine();
        m_waveInProgress = false;
    }
    // increase threat level
    public void IncreaseThreat() 
    {
        // Increment to spawn next wave
        m_currentThreatStage = Mathf.Min(m_currentThreatStage + 1, m_routineSpawnStages.Count - 1);
        OnStart?.Invoke(m_currentThreatStage);
    }
    public void DecreseThreat() 
    {
        // Increment to spawn next wave
        m_currentThreatStage = Mathf.Max(m_currentThreatStage - 1, 0);
        OnStart?.Invoke(m_currentThreatStage);
    }
    // spawn one wave of enemies
    IEnumerator RoutineSpawn_Internal() 
    {
        while (m_waveInProgress) 
        {
            SpawnWave wave = m_routineSpawnStages[m_currentThreatStage];
            float delay = wave.SecondsBeforeSpawn < 0f ? 60f : wave.SecondsBeforeSpawn;
            // spawn a wave of enemies
            SpawnWave(wave);
            // after spawning, wait for a set duration
            yield return new WaitForSeconds(delay);
        }             
    }
    void PrepareEnenmy(Enemy spawned) 
    {
        spawned.Init(m_defaultAggroTarget);
        spawned.OnDefeated += (a) => { m_onEnemyDefeat?.Invoke(a); }; 
        m_onEnemySpawn?.Invoke(spawned);
    }
}