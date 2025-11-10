using UnityEngine;

public class Stun : EffectModule
{
    [Range(0f, 60f)]
    [SerializeField] float m_duration = default;
    public override void Activate(BaseEntity target)
    {
        base.Activate(target);
        target?.GetComponent<Stunnable>()?.Stun(m_duration);
    }
    public override void Deactivate(BaseEntity target)
    {
        base.Deactivate(target);
    }
}
