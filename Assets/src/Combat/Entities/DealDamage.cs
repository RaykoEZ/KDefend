using UnityEngine;

public class DealDamage : EffectModule
{
    [SerializeField] bool m_nonLethal = default;
    [SerializeField] protected int m_damage = default;
    public override void Activate(BaseEntity target)
    {
        base.Activate(target);
        // check for lethal damage
        int damage = m_nonLethal? Mathf.Min(target.CurrentStats.Property.Health - 1, m_damage) : m_damage;            
        target?.TakeDamage(damage);
    }
}