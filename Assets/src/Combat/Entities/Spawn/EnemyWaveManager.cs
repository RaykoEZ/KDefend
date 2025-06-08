using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] int m_spawnCountForCooldown = default;
    [SerializeField] Transform m_spawnParent = default;
    [SerializeField] ThreatHandler m_threat = default;
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
    [SerializeField] RoutineCaller m_routineSpawn = default;
    bool m_waveInProgress = false;
    // spawn counter for spawner cooldown
    int m_spawnCounter = 0;
    void Start()
    {
        m_routineSpawn.OnNewInterval += OnSpawnWaveInterval;
        RoutineWave();
    }
    void OnSpawnWaveInterval()
    {
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
    // trigger static spawn with scene/script events
    // return spawn groups
    public int StaticSpawn(int spawnIndex) 
    {
        int numEnemies = 0;
        if (spawnIndex > 0 && spawnIndex < m_staticSpawns.Count) 
        {
            numEnemies = SpawnWave(m_staticSpawns[spawnIndex]);
        }
        return numEnemies;
    }
    // return number of enemies spawned in a group
    public virtual int SpawnGroup(SpawnGroup newGroup) 
    {
        int numEnemies = 0;
        int i = newGroup.SpawnLocationIndex > 0 &&
                newGroup.SpawnLocationIndex < m_spawners.Count ?
                newGroup.SpawnLocationIndex : 0;
        foreach (var spawn in newGroup.GroupsToSpawn.Items)
        {
            var enemyRef = m_enemyRefs.GetEnemyRef(spawn.SpawnRef);
            // spawn the group
            m_spawners[i].Spawn(enemyRef, m_spawnParent, spawn.NumToSpawn, 0.1f, PrepareEnenmy);
            numEnemies++;
        }
        return numEnemies;
    }
    // returns number of enemies spawned from all spawned groups
    protected int SpawnWave(SpawnWave wave) 
    {
        int numEnemies = 0;
        foreach (var group in wave.EnemyGroups)
        {
            numEnemies += SpawnGroup(group);
        }
        return numEnemies;
    }
    public void StopRoutineWave()
    {
        if (!m_waveInProgress) return;
        m_routineSpawn.StopRoutine();
        m_waveInProgress = false;
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
        if (m_spawnCounter >= m_spawnCountForCooldown)
        {
            yield return new WaitForSeconds(10f);
            m_spawnCounter = 0;
        }
    }
    void PrepareEnenmy(Enemy spawned) 
    {
        spawned.Init(m_defaultAggroTarget);
        spawned.OnDefeated += (a) => { m_onEnemyDefeat?.Invoke(a); }; 
        m_onEnemySpawn?.Invoke(spawned);
    }
}