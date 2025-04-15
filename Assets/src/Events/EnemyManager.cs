using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour 
{
    [SerializeField] Transform m_spawnParent = default;
    [SerializeField] Enemy m_scoutRef = default;
    [SerializeField] Enemy m_agentRef = default;
    // waves triggered by game event
    [SerializeField] List<SpawnCollection> m_staticSpawns = default;
    // frequent spawns bound by timer, depending on game state
    [SerializeField] List<SpawnCollection> m_routineSpawns = default;
    List<Enemy> m_activeEnemies = default;
    Dictionary<int, SpawnContainer> m_staticPool = new Dictionary<int, SpawnContainer>();
    Dictionary<int, SpawnContainer> m_routinePool = new Dictionary<int, SpawnContainer>();

    public IReadOnlyList<Enemy> ActiveEnemies { get => m_activeEnemies;}
    public List<EnemyState> EnemyStates() 
    {
        var ret = new List<EnemyState>();
        foreach (var item in ActiveEnemies)
        {
            ret.Add(item.State);
        }
        return ret;
    }
    public void Init(List<EnemyState> newState) 
    {
        // setup spawn pools
        m_staticPool = SpawnCollection.CreateCollections(m_staticSpawns);
        m_routinePool = SpawnCollection.CreateCollections(m_routineSpawns);
        // clear all previous enemies and respawn according to new state
        foreach (var item in m_activeEnemies)
        {
            item?.Despawn();
        }
        m_activeEnemies.Clear();
        SpawnEnemies(newState);
    }
    public void SpawnNewWave() 
    { 
    
    }
    public void SpawnEnemies(List<EnemyState> states) 
    {
        Enemy spawnRef;
        foreach (var item in states)
        {
            spawnRef = GetSpawnRef(item);
            var instance = GameUtil.SpawnObject(spawnRef,
                item.State.Position, m_spawnParent);
            m_activeEnemies.Add(instance);
            instance.OnDefeated += OnEnemyDefeated;
        }
    }
    protected Enemy GetSpawnRef(EnemyState state) 
    {
        Enemy ret = null;
        switch (state.Type)
        {
            case EnemyType.Scout:
                ret = m_scoutRef;
                break;
            case EnemyType.Agent:
                ret = m_agentRef;
                break;
            case EnemyType.Command:
                break;
            default:
                ret = m_scoutRef;
                break;
        }
        return ret;
    }
    public void OnEnemyDefeated(Enemy spawned) 
    {
        if (spawned == null) return;
        m_activeEnemies.Remove(spawned);
    }
    public void OnEnemySpawned(Enemy spawned) 
    {
        if (spawned == null) return;
        m_activeEnemies.Add(spawned);
    }
}
