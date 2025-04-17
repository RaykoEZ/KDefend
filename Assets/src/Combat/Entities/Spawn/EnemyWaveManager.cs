using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public delegate void OnWaveStart(int waveNumber);
public class EnemyWaveManager : MonoBehaviour
{
    // waves triggered by game event
    [SerializeField] List<SpawnWave> m_staticSpawns = default;
    // frequent spawns bound by timer, depending on game state
    [SerializeField] List<SpawnWave> m_routineSpawns = default;
    // Spawner in locations
    [SerializeField] List<EnemySpawner> m_spawners = default;
    [SerializeField] UnityEvent<Enemy> m_onEnemySpawn = default;
    [SerializeField] UnityEvent<Enemy> m_onEnemyDefeat= default;
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
        m_waveSpawn.ScheduleCoroutine(RoutineSpawn_Internal(m_routineSpawns), true);
    }
    // trigger static spawn with scene/script events
    public void StaticSpawn(int spawnIndex) 
    { 
        if (spawnIndex > 0 && spawnIndex < m_staticSpawns.Count) 
        {
            SpawnWave(m_staticSpawns[spawnIndex]);
        }
    }
    public void StopWave() 
    {
        if (!m_waveInProgress) return;
        m_waveSpawn.StopCurrentCoroutine();
        m_waveInProgress = false;
    }
    public virtual void SpawnGroup(SpawnGroup newGroup) 
    {
        int i = newGroup.SpawnLocationIndex > 0 &&
                newGroup.SpawnLocationIndex < m_spawners.Count ?
                newGroup.SpawnLocationIndex : 0;
        foreach (var spawn in newGroup.GroupsToSpawn.Items)
        {
            // spawn the group
            m_spawners[i].Spawn(spawn.SpawnRef, spawn.NumToSpawn, 0.1f, PrepareEnenmy);
        }
    }
    protected void SpawnWave(SpawnWave wave) 
    {
        foreach (var group in wave.EnemyGroups)
        {
            SpawnGroup(group);
        }
    }
    // spawn one wave of enemies
    IEnumerator RoutineSpawn_Internal(List<SpawnWave> waves) 
    {
        foreach (var item in waves)
        {
            m_currentWave++;
            OnStart?.Invoke(m_currentWave + 1);
            yield return new WaitForSeconds(item.SecondsBeforeSpawn);
            // spawn a wave of enemies
            SpawnWave(item);
            // neegative delay = pause
            if (item.SecondsBeforeSpawn < 0f)
            {
                StopWave();
                yield break;
            }
            // after spawning, wait for a set duration
            yield return new WaitForSeconds(item.SecondsBeforeSpawn);
            // Increment to spawn next wave
        }           
        m_waveInProgress = false;
    }
    void PrepareEnenmy(Enemy spawned) 
    {
        spawned.Init(m_defaultAggroTarget);
        spawned.OnDefeated += (a) => { m_onEnemyDefeat?.Invoke(a); }; 
        m_onEnemySpawn?.Invoke(spawned);
    }
}
