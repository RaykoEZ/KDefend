using System;
using System.Collections;
using UnityEngine;
// Handles enemy spawning
public class EnemySpawner : EntitySpawner<Enemy>
{
    [SerializeField] RangedSpawner m_spawner = default;
    public override void Spawn(
        Enemy spawnRef, 
        int numToSpawn = 1, 
        float spawnDelayInterval = 0.1f, 
        Action<Enemy> onSpawnAction = null) 
    {
        StartCoroutine(Spawn_Internal(spawnRef, numToSpawn, spawnDelayInterval, onSpawnAction));
    }
    IEnumerator Spawn_Internal(Enemy spawnRef, int numToSpawn = 1, float spawnDelayInterval = 0.1f, Action<Enemy> onSpawnAction = null) 
    {
        int numSpawned = 0;
        while(numSpawned < numToSpawn) 
        {
            yield return new WaitForSeconds(spawnDelayInterval);
            Enemy instance = m_spawner.Spawn(spawnRef);
            onSpawnAction?.Invoke(instance);
            numSpawned++;
        }
            
    }
}
