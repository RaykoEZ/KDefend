using System.Collections.Generic;
using UnityEngine;
// The container of one spawn wave, consists of spawn on multiple locations
[CreateAssetMenu(fileName = "spwnWave_", menuName = "Jams/EnemySpawning/SpawnWave", order = 0)]
public class SpawnWave : ScriptableObject
{
    [SerializeField] float m_secondsBeforeNextWave = default;
    [SerializeField] List<SpawnGroup> m_enemyGroups = default;
    // < 0f for stop spawning
    public float SecondsBeforeSpawn => m_secondsBeforeNextWave;
    public IReadOnlyList<SpawnGroup> EnemyGroups { get => m_enemyGroups; }
    public static Dictionary<int, SpawnContainer> CreateDictionary(IReadOnlyList<SpawnGroup> enemyGroup)
    {
        Dictionary<int, SpawnContainer> ret = new Dictionary<int, SpawnContainer>();
        SpawnContainer c;
        foreach (var item in enemyGroup)
        {
            c = item.GroupsToSpawn;
            if (!ret.TryGetValue(item.SpawnLocationIndex, out SpawnContainer result))
            {
                ret.Add(item.SpawnLocationIndex, c);
            }
            else 
            {
                result?.AddRange(c.Items);
            }
        }
        return ret;
    }
}