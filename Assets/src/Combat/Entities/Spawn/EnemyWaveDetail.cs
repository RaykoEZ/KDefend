using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemySpawnItem
{
    [SerializeField] int m_numToSpawn;
    // if < 0, pick random spawn location from list
    [SerializeField] int m_spawnLocationIndex;
    [SerializeField] Enemy m_spawnRef;
    public int NumToSpawn => m_numToSpawn;
    public Enemy SpawnRef => m_spawnRef;
    public int SpawnLocationIndex => m_spawnLocationIndex;
}
//Contains all enemies to spawn in a wave
[CreateAssetMenu(fileName = "Wave_", menuName = "Jams/EnemySpawning/Create a new enemy wave detail", order = 1)]
public class EnemyWaveDetail : ScriptableObject 
{
    // time to wait before next wave, start counting down after we spawned all enemies in this wave 
    [SerializeField] float m_secondsBeforeNextWave = default;
    // A group of enemies spawn from the same spawner location
    [SerializeField] List<EnemySpawnItem> m_groupsToSpawn = default;
    public float SecondsBeforeNextWave => m_secondsBeforeNextWave;
    public List<EnemySpawnItem> GroupsToSpawn => m_groupsToSpawn;
}

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
public class SpawnContainer : MonoBehaviour 
{

    Dictionary<int, List<EnemySpawnItem>> m_spawnsByLocation = new Dictionary<int, List<EnemySpawnItem>>();
    // use for timer spawns
    Dictionary<int, List<SpawnId>> m_spawnBytimeFrame = new Dictionary<int, List<SpawnId>>();
}