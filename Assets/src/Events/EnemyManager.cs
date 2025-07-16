using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// handles all enemy state and
// spawns all pre-existing enemies from save data
public class EnemyManager : MonoBehaviour 
{
    [SerializeField] Transform m_spawnParent = default;
    [SerializeField] ThreatHandler m_threat = default;
    [SerializeField] EnemyAssetList m_enemyRefs = default;
    [SerializeField] UnityEvent<Enemy> m_onDefeated = default;
    List<Enemy> m_activeEnemies = new List<Enemy>();
    static int s_activeEnemyCount;
    public IReadOnlyList<Enemy> ActiveEnemies { get => m_activeEnemies; }
    public static int ActiveEnemyCount => s_activeEnemyCount;
    public List<EnemyState> GetEnemyStates() 
    {
        var ret = new List<EnemyState>();
        foreach (var item in ActiveEnemies)
        {
            ret.Add(item.State);
        }
        return ret;
    }
    public void Init(int threat, List<EnemyState> newState) 
    {
        m_threat.SetThreatStage(threat);
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
            spawnRef = m_enemyRefs.GetEnemyRef(item.EnemyIndex);
            var instance = GameUtil.SpawnObject(spawnRef,
                item.State.Position, m_spawnParent);
            m_activeEnemies.Add(instance);
            // set up enemy type index
            instance?.SetEnemyType(item.EnemyIndex);
            instance.OnDefeated += OnEnemyDefeated;
        }
    }
    public void OnEnemyDefeated(Enemy spawned) 
    {
        if (spawned == null) return;
        // increase threat value on kill, trigger defeat events
        m_threat?.UpdateThreat(spawned.ThreatIncrease);
        m_onDefeated?.Invoke(spawned);
        m_activeEnemies.Remove(spawned);
        s_activeEnemyCount--;
    }
    public void OnEnemySpawned(Enemy spawned) 
    {
        if (spawned == null) return;
        s_activeEnemyCount++;
        m_activeEnemies.Add(spawned);
    }
}
