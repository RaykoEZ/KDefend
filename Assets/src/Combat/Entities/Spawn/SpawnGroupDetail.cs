using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemySpawnItem
{
    [SerializeField] int m_numToSpawn;
    // if < 0, pick random spawn location from list
    [SerializeField] Enemy m_spawnRef;
    public int NumToSpawn => m_numToSpawn;
    public Enemy SpawnRef => m_spawnRef;
}
//Contains all enemies to spawn in a wave
[CreateAssetMenu(fileName = "Wave_", menuName = "Jams/EnemySpawning/Create a new enemy wave detail", order = 1)]
public class SpawnGroupDetail : ScriptableObject 
{
    // time to wait before next wave, start counting down after we spawned all enemies in this wave 
    [SerializeField] int m_spawnLocationIndex;
    [SerializeField] float m_secondsBeforeNextWave = default;

    // A group of enemies spawn from the same spawner location
    [SerializeField] List<EnemySpawnItem> m_groupsToSpawn = default;
    public int SpawnLocationIndex => m_spawnLocationIndex;
    public float SecondsBeforeNextWave => m_secondsBeforeNextWave;
    public SpawnContainer GroupsToSpawn => new SpawnContainer(m_groupsToSpawn);
}
// Index for the list in spawn dictionary in SpawnContainer
public struct SpawnId : IEquatable<SpawnId>
{
    int m_location;
    int m_indexInList;
    public int Location { get => m_location; }
    public int IndexInList { get => m_indexInList; }
    public bool Equals(SpawnId other)
    {
        return Location == other.Location && IndexInList == other.IndexInList;
    }
}
[Serializable]
public class SpawnContainer 
{
    [SerializeField] List<EnemySpawnItem> m_items = new List<EnemySpawnItem>();
    public IReadOnlyList<EnemySpawnItem> Items => m_items;
    public SpawnContainer(SpawnContainer copy) 
    {
        m_items = new List<EnemySpawnItem>(copy.m_items);
    }
    public SpawnContainer(IReadOnlyList<EnemySpawnItem> list) 
    {
        m_items = new List<EnemySpawnItem>(list);
    }
    public void AddRange(IReadOnlyList<EnemySpawnItem> add) 
    {
        m_items.AddRange(add);
    }
}
