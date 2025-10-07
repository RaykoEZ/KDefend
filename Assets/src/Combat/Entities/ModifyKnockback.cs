using UnityEngine;

public class ModifyKnockback : EffectModule
{
    [Range(0.1f, 2f)]
    [SerializeField] float m_knockbackMultiplier = default;
    public override void Activate(BaseEntity target)
    {
        target?.ModifyKnockback(m_knockbackMultiplier);
    }
    public override void Deactivate(BaseEntity target)
    {
        target?.ModifyKnockback(-m_knockbackMultiplier);
    }
}

