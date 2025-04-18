using System.Collections.Generic;
using UnityEngine;
// list of enemy prefabs to reference for spawning
// index corresponds to enemy type
[CreateAssetMenu(fileName = "EnemyPool_", menuName = "KDefender/Spawn/New Enemy Asset List")]
public class EnemyAssetCollection : ScriptableObject 
{
    [SerializeField] List<Enemy> m_enemyAssets = default;
    public IReadOnlyList<Enemy> EnemyAssets => m_enemyAssets;
    public Enemy GetSpawnRef(EnemyType enemyType)
    {
        Enemy ret = m_enemyAssets[(int)enemyType];
        return ret;
    }
}
