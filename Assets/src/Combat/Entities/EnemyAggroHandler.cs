using System.Collections.Generic;
using UnityEngine;
// logs a set of enemies and aggro on entities
// Allows aggro control on map events
public class EnemyAggroHandler : MonoBehaviour 
{
    static HashSet<Enemy> m_currentAggroList = new HashSet<Enemy>();
    public static void Add(Enemy enemy) 
    {
        m_currentAggroList?.Add(enemy);
    }
    public static void Remove(Enemy enemy)
    {
        m_currentAggroList?.Remove(enemy);
    }
    public static void ResetAllAggroState() 
    {
        foreach (var item in m_currentAggroList)
        {
            item?.ResetTarget();
        }
    }
    public static void ResetAggroState(Enemy enemy) 
    {
        if (m_currentAggroList.Contains(enemy)) 
        {
            enemy?.ResetTarget();
        }
    }
    public static void SetAllAggroActive(bool enable = true) 
    {
        foreach (var item in m_currentAggroList)
        {
            SetAggroActive(item, enable);
        }
    }
    public static void SetAggroActive(Enemy enemy, bool enable) 
    {
        enemy?.SetAggro(enable);
    }
}
