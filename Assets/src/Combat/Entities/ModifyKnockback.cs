using UnityEngine;

public class ModifyKnockback : EffectModule
{
    [Range(0.1f, 10f)]
    [SerializeField] float m_knockbackMultiplier = default;
    public override void Activate(BaseEntity target)
    {
        base.Activate(target);
        target?.ModifyKnockback(m_knockbackMultiplier);
    }
    public override void Deactivate(BaseEntity target)
    {
        base.Deactivate(target);
        target?.ModifyKnockback(-m_knockbackMultiplier);
    }
}