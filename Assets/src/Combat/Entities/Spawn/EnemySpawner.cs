using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
// Handles enemy spawning
public class EnemySpawner : EntitySpawner<Enemy>
{
    [SerializeField] RangedSpawner m_spawner = default;
    public override void Spawn(
        Enemy spawnRef,
        Transform parent,
        int numToSpawn = 1, 
        float spawnDelayInterval = 0.1f, 
        Action<Enemy> onSpawnAction = null) 
    {
        StartCoroutine(Spawn_Internal(spawnRef, parent, numToSpawn, spawnDelayInterval, onSpawnAction));
    }
    IEnumerator Spawn_Internal(Enemy spawnRef, Transform parent, int numToSpawn = 1, float spawnDelayInterval = 0.1f, Action<Enemy> onSpawnAction = null) 
    {
        int numSpawned = 0;
        while(numSpawned < numToSpawn) 
        {
            yield return new WaitForSeconds(spawnDelayInterval);
            Enemy instance = m_spawner.Spawn(spawnRef, parent);
            onSpawnAction?.Invoke(instance);
            numSpawned++;
        }
            
    }
}
