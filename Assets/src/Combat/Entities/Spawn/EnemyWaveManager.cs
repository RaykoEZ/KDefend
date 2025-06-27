using Curry.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] int m_stopRoutineSpawnOnSpawnCount = default;
    [SerializeField] Transform m_spawnParent = default;
    [SerializeField] ThreatHandler m_threat = default;
    [SerializeField] EnemyAssetList m_enemyRefs = default;
    // frequent spawns bound by timer, depending on threat level and game state trigger
    [SerializeField] List<SpawnWave> m_routineSpawnStages = default;
    // Spawner in locations
    [SerializeField] List<EnemySpawner> m_spawners = default;
    [SerializeField] UnityEvent<Enemy> m_onEnemySpawn = default;
    [SerializeField] UnityEvent<Enemy> m_onEnemyDefeat = default;
    [SerializeField] BaseEntity m_defaultAggroTarget = default;
    [SerializeField] RoutineCaller m_routineSpawn = default;
    bool m_waveInProgress = false;
    void Start()
    {
        // setup routine call
        m_routineSpawn.OnNewInterval += OnSpawnWaveInterval;
        RoutineWave();
    }
    void OnSpawnWaveInterval()
    {
        if (EnemyManager.ActiveEnemyCount > m_stopRoutineSpawnOnSpawnCount) 
        {
            StopRoutineWave();
            StartCoroutine(DelayRoutineWave());
        }
        ThreatScalings threat = m_threat.GetCurrentThreatMultiplier();
        float delay = threat.TimeBetweenRoutineWave <= 0f ? 30f :
                    threat.TimeBetweenRoutineWave;
        m_routineSpawn.TimeInterval = delay;
    }

    public void RoutineWave() 
    {
        if (m_waveInProgress) return;
        m_waveInProgress = true;
        //make coroutine handle regular spawns
        m_routineSpawn.StartRoutine(RoutineSpawn_Internal());
    }
    // catches spawn events from miscellaneous entities
    public void SpawnCustomWave(EventInfo eventInfo) 
    {
        if (eventInfo == null || eventInfo.Payload == null) return;
        if (eventInfo.Payload.TryGetValue("wave", out object result) 
            && result is SpawnWave wave) 
        {
            SpawnWave(wave);
        }
    }
    // return number of enemies spawned in a group
    protected virtual int SpawnGroup(SpawnGroup newGroup) 
    {
        int numEnemies = 0;
        // random spawn location if location index is -1 or over max index
        int i = newGroup.SpawnLocationIndex > 0 &&
                newGroup.SpawnLocationIndex < m_spawners.Count ?
                newGroup.SpawnLocationIndex : UnityEngine.Random.Range(0, m_spawners.Count);
        foreach (var spawn in newGroup.GroupsToSpawn.Items)
        {
            var enemyRef = m_enemyRefs.GetEnemyRef(spawn.SpawnRef);
            // spawn the group
            m_spawners[i].Spawn(enemyRef, m_spawnParent, spawn.NumToSpawn, 3f, PrepareEnenmy);
            numEnemies++;
        }
        return numEnemies;
    }
    // returns number of enemies spawned from all spawned groups
    public void SpawnWave(SpawnWave wave) 
    {
        // check for number of enemies already on field beofre spawning more
        if (EnemyManager.ActiveEnemyCount > m_stopRoutineSpawnOnSpawnCount) return;

        StartCoroutine(SpawnWave_Internal(wave));       
    }
    IEnumerator SpawnWave_Internal(SpawnWave wave) 
    {
        foreach (var group in wave.EnemyGroups)
        {
            SpawnGroup(group);
            yield return SpawnCooldown();
        }
    }
    public void StopRoutineWave()
    {
        if (!m_waveInProgress) return;
        m_routineSpawn.StopRoutine();
        m_waveInProgress = false;
    }
    // Wait until active enemy count drops below threshold to continue routine spawn 
    IEnumerator DelayRoutineWave() 
    {
        Debug.Log("Routine Wave stopped, waiting for player to defeat current enemies");
        yield return new WaitUntil(() => EnemyManager.ActiveEnemyCount < m_stopRoutineSpawnOnSpawnCount);
        // start spawning again
        yield return SpawnCooldown();
        RoutineWave();
    }
    // spawn one wave of enemies
    IEnumerator RoutineSpawn_Internal() 
    {
        // wait for a set duration if spawn count exceeded
        yield return SpawnCooldown();
        // spawn a wave of enemies
        SpawnWave wave = m_routineSpawnStages[m_threat.CurrentThreat];
        SpawnWave(wave);
    }
    IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(5f);
    }
    void PrepareEnenmy(Enemy spawned) 
    {
        spawned.Init(m_defaultAggroTarget);
        spawned.OnDefeated += (a) => { m_onEnemyDefeat?.Invoke(a); }; 
        m_onEnemySpawn?.Invoke(spawned);
    }
}