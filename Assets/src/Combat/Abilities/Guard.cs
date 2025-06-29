using System.Collections.Generic;
using UnityEngine;

public class Guard : AbilityHandler
{
    [SerializeField] ReflectProjectile m_reflect = default;
    [SerializeField] ShieldBash m_bash = default;
    [SerializeField] RangeDetector m_attackRadius = default;
    public override List<ActiveAbility> Abilities => new List<ActiveAbility> { m_reflect, m_bash };
    protected BaseEntity Target => (Self as Enemy).CurrentTarget;
    void FixedUpdate()
    {
        GuardSkillCheck();
    }
    public override void OnTakeHit()
    {
        base.OnTakeHit();
        GuardSkillCheck();     
    }
    // Reflects colliding projectiles for X seconds, affects both enemy & player
    void GuardSkillCheck()
    {
        if (m_attackRadius.IsInRange(Target))
        {
            m_bash?.TryUse();
        }
        else
        {
            m_reflect?.TryUse();
        }
    }
}
