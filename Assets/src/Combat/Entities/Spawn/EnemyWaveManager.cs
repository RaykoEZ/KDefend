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
    void OnEnable()
    {
        KDEventHandler.ListenToGlobal(GameEventTriggerType.Spawn, OnGlobalSpawnEvent);
    }
    void OnDisable()
    {
        KDEventHandler.UnlistenFromGlobal(GameEventTriggerType.Spawn, OnGlobalSpawnEvent);
    }
    void Start()
    {
        // setup routine call
        m_routineSpawn.OnNewInterval += OnSpawnWaveInterval;
    }
    void OnGlobalSpawnEvent(object sender, KDEventInfo args) 
    {
        if (args.Payload == null) return;
        if (args.Payload.TryGetValue("wave", out object result) && result is SpawnWave wavw)
        {
            SpawnWave(wavw);
        }
    }
    void OnSpawnWaveInterval()
    {
        if (EnemyManager.ActiveEnemyCount > m_stopRoutineSpawnOnSpawnCount) 
        {
            StopRoutineWave();
            StartCoroutine(DelayRoutineWave());
        }
        m_routineSpawn.TimeInterval = UnityEngine.Random.Range(15f, 30f);
    }
    public void RoutineWave() 
    {
        if (m_waveInProgress) return;
        m_waveInProgress = true;
        RoutineSpawn_Internal();
        //make coroutine handle regular spawns
        m_routineSpawn.StartRoutine(RoutineSpawn_Internal);
    }
    // spawn one wave of enemies
    void RoutineSpawn_Internal()
    {
        // spawn a wave of enemies
        int index = Mathf.Clamp((int)DayCounter.Current, 0, m_routineSpawnStages.Count);
        SpawnWave wave = m_routineSpawnStages[index];
        SpawnWave(wave);
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
            m_spawners[i].Spawn(enemyRef, m_spawnParent, spawn.NumToSpawn, 0.5f, PrepareEnemy);
            numEnemies++;
        }
        return numEnemies;
    }
    public void SpawnWave(SpawnWave wave) 
    {
        if (wave == null || wave.EnemyGroups.Count == 0) return;
        foreach (var group in wave.EnemyGroups)
        {
            SpawnGroup(group);
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
        yield return new WaitUntil(() => EnemyManager.ActiveEnemyCount < m_stopRoutineSpawnOnSpawnCount);
        // start spawning again
        yield return SpawnCooldown();
        RoutineWave();
    }

    IEnumerator SpawnCooldown()
    {
        float wait = UnityEngine.Random.Range(2f, 6f);
        yield return new WaitForSeconds(wait);
    }
    void PrepareEnemy(Enemy spawned) 
    {
        spawned.InitTarget(m_defaultAggroTarget);
        spawned.OnDefeated += (a) => { m_onEnemyDefeat?.Invoke(a); }; 
        m_onEnemySpawn?.Invoke(spawned);
    }
}