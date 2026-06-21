using UnityEngine;

public class DealDamage : EffectModule
{
    [SerializeField] protected int m_damage = default;
    public override void Activate(BaseEntity target)
    {
        m_onActivate?.Invoke(target);
        target?.TakeDamage(m_damage);
    }
}