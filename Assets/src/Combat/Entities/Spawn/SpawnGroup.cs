using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemySpawn
{
    [SerializeField] int m_numToSpawn;
    // if < 0, pick random spawn location from list
    [SerializeField] int m_enemyRefIndex;
    public int NumToSpawn => m_numToSpawn;
    public int SpawnRef => m_enemyRefIndex;
}
[Serializable]
//Contains all enemies in a group of spawn location
public class SpawnGroup 
{
    // time to wait before next wave, start counting down after we spawned all enemies in this wave 
    [SerializeField] int m_spawnLocationIndex;
    // A group of enemies spawn from the same spawner location
    [SerializeField] List<EnemySpawn> m_groupsToSpawn = default;
    public int SpawnLocationIndex => m_spawnLocationIndex;
    public SpawnContainer GroupsToSpawn => new SpawnContainer(m_groupsToSpawn);
}
[Serializable]
public class SpawnContainer 
{
    [SerializeField] List<EnemySpawn> m_items = new List<EnemySpawn>();
    public IReadOnlyList<EnemySpawn> Items => m_items;
    public SpawnContainer(SpawnContainer copy) 
    {
        m_items = new List<EnemySpawn>(copy.m_items);
    }
    public SpawnContainer(IReadOnlyList<EnemySpawn> list) 
    {
        m_items = new List<EnemySpawn>(list);
    }
    public void AddRange(IReadOnlyList<EnemySpawn> add) 
    {
        m_items.AddRange(add);
    }
}
