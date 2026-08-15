using UnityEngine;

public class SummonEffect : EffectModule
{
    [Range(1, 99)]
    [SerializeField] int m_numToSummon = default;
    [SerializeField] SummonedEntity m_toSpawn = default;
    public override void Activate(BaseEntity target)
    {
        SummonedEntity instance;
        for (int i = 0; i < m_numToSummon; i++)
        {
            instance = GameUtil.SpawnObject(m_toSpawn, transform.position, transform.parent);
            instance?.Init();
        }
    }
}
