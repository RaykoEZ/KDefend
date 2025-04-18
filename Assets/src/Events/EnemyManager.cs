using System.Collections.Generic;
using UnityEngine;

// handles all enemy state and
// spawns all pre-existing enemies from save data
public class EnemyManager : MonoBehaviour 
{
    [SerializeField] Transform m_spawnParent = default;
    [SerializeField] EnemyAssetCollection m_enemyRefs = default;
    List<Enemy> m_activeEnemies = default;
    public IReadOnlyList<Enemy> ActiveEnemies { get => m_activeEnemies;}
    public List<EnemyState> GetEnemyStates() 
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
        // clear all previous enemies and respawn according to new state
        foreach (var item in m_activeEnemies)
        {
            item?.Despawn();
        }
        m_activeEnemies.Clear();
        SpawnEnemies(newState);
    }
    public void SpawnEnemies(List<EnemyState> states) 
    {
        Enemy spawnRef;
        foreach (var item in states)
        {
            spawnRef = m_enemyRefs.GetSpawnRef(item.Type);
            var instance = GameUtil.SpawnObject(spawnRef,
                item.State.Position, m_spawnParent);
            m_activeEnemies.Add(instance);
            instance.OnDefeated += OnEnemyDefeated;
        }
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
