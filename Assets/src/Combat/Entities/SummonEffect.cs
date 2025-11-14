using UnityEngine;

public class SummonEffect : EffectModule
{
    [SerializeField] SummonedEntity m_toSpawn = default;
    public override void Activate(BaseEntity target)
    {
        var instance = GameUtil.SpawnObject(m_toSpawn, transform.position, transform.parent);
        instance?.Init();
    }
}
