using System.Collections.Generic;
using UnityEngine;
//enemy agent behaviour
[RequireComponent(typeof(Enemy))]
public class Agent : MonoBehaviour
{
    [SerializeField] int m_numPerSummon = default;
    [SerializeField] Enemy m_toSummon = default;
    [SerializeField] EnemySpawner m_spawner = default;
    public virtual void SummonGrunts() 
    {
        m_spawner?.Spawn(m_toSummon, m_numPerSummon, 0.5f, CommandGrunts);
    }
    protected virtual void CommandGrunts(Enemy toCommand) 
    {
        Enemy self = GetComponent<Enemy>();
        toCommand?.Init(self.TargetsOfInterest as List<BaseEntity>, self.DefaultTarget);
    }
}
