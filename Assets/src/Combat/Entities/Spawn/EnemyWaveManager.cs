using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public delegate void OnWaveStart(int waveNumber);
public delegate void OnAllWaveCleared();
public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] List<EnemyWaveDetail> m_waves = default;
    [SerializeField] List<EnemySpawner> m_spawners = default;
    [SerializeField] List<BaseEntity> m_targetPriorityList = default;
    Coroutine m_spawnWave;
    public event OnAllWaveCleared OnAllCleared;
    public event OnWaveStart OnStart;
    // current wave number
    int m_currentWave = 0;
    int m_numEnemies = 0;
    bool m_waitingForNextWave = false;
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
        if (m_spawnWave != null) return;
        m_spawnWave = StartCoroutine(Spawn_Internal());
    }
    public void PauseWave() 
    {
        if (m_spawnWave == null) return;
        StopCoroutine(m_spawnWave);
        m_waitingForNextWave = false;
        m_spawnWave = null;
    }
    IEnumerator Spawn_Internal() 
    {
        while (m_currentWave < m_waves.Count) 
        {
            EnemyWaveDetail wave = m_waves[m_currentWave];
            // Choose random spawners from list of spawner
            List<EnemySpawner> spawners = SamplingUtil.SampleFromList(
                m_spawners, wave.GroupsToSpawn.Count, uniqueResults: true);
            int i = 0;
            OnStart?.Invoke(m_currentWave + 1);
            // spawn reward
            // wait for wave countdown
            yield return new WaitForSeconds(3f);
            foreach (var group in wave.GroupsToSpawn)
            {
                // spawn the group
                spawners[i].Spawn(group.SpawnRef, group.NumToSpawn, 0.1f, InitEnenmy);
                // increment enemy count
                m_numEnemies += group.NumToSpawn;
                i++;
                yield return new WaitForSeconds(0.9f);
            }
            // after spawning, wait for a set duration
            m_waitingForNextWave = true;
            yield return new WaitForSeconds(wave.SecondsBeforeNextWave);
            m_waitingForNextWave = false;
            // Increment to spawn next wave
            m_currentWave++;
        }
        m_spawnWave = null;
    }
    void InitEnenmy(Enemy spawned) 
    {
        spawned.Init(m_targetPriorityList);
        spawned.OnDefeated += OnEnemyDefeated;
    }
    void OnEnemyDefeated(Enemy defeated) 
    {
        defeated.OnDefeated -= OnEnemyDefeated;
        m_numEnemies--;
        // if we cleared the wave early
        if(m_numEnemies == 0 && m_currentWave < m_waves.Count && m_waitingForNextWave) 
        {
            m_waitingForNextWave = false;
            PauseWave();
            m_currentWave++;
            StartWave();
        }
        else if (m_numEnemies == 0 && m_currentWave >= m_waves.Count) 
        {
            OnAllCleared?.Invoke();
        }
    }
}
