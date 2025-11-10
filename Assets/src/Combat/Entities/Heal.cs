using UnityEngine;

public class Heal : EffectModule
{
    [SerializeField] protected int m_healAmount = default;
    public override void Activate(BaseEntity target)
    {
        base.Activate(target);
        target?.Heal(m_healAmount);
    }
}