using UnityEngine;
public class SpeedEffect : EffectModule
{
    [Range(-1f, 1f)]
    [SerializeField] float m_change = default;
    public override void Activate(BaseEntity target)
    {
        target?.ModifySpeed(m_change);
    }
    public override void Deactivate(BaseEntity target)
    {
        target?.ModifySpeed(-m_change);
    }
}

