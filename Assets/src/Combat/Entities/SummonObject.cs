using UnityEngine;

public class SummonObject : EffectModule
{
    [SerializeField] GameObject m_toSpawn = default;
    public override void Activate(BaseEntity target)
    {
        Instantiate(m_toSpawn, transform.parent, true);
    }
}

