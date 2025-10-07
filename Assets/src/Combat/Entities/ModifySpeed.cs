using UnityEngine;

public class ModifySpeed : EffectModule 
{
    [Range(0.1f, 2f)]
    [SerializeField] float m_baseSpeedGainMultiplier = default;
    public override void Activate(BaseEntity target) 
    {
        target?.ModifySpeed(m_baseSpeedGainMultiplier);
    }
    public override void Deactivate(BaseEntity target) 
    {
        target?.ModifySpeed(-m_baseSpeedGainMultiplier);
    }
}
