using UnityEngine;

public class Charm : EffectModule
{
    [Range(0f, 10f)]
    [SerializeField] float m_duration = default;
    public override void Activate(BaseEntity target)
    {
        base.Activate(target);
        target?.GetComponent<Charmable>()?.Charm(m_duration);
    }
}
