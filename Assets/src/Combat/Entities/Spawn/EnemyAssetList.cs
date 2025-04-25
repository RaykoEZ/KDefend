using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EList_", menuName = "Jams/New list of available enemies", order = 1)]
public class EnemyAssetList : ScriptableObject 
{
    [SerializeField] List<Enemy> m_enemyRefs = default;
    public IReadOnlyList<Enemy> Enemies { get => m_enemyRefs; }
    public Enemy GetEnemyRef(int enemyIndex) 
    {
        if (enemyIndex < 0 || m_enemyRefs.Count <= enemyIndex) return null;
        return m_enemyRefs[enemyIndex];
    }
}
