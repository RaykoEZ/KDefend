using UnityEngine;

public class BreathAttack : EffectModule 
{
    [SerializeField] ParticleSystem m_breath = default;
    public override void Activate(BaseEntity target)
    {
        m_breath?.Play();
        base.Activate(target);
    }
    public override void Deactivate(BaseEntity target) 
    {
        m_breath?.Stop();
        base.Deactivate(target);
    }
}