using System.Collections.Generic;
using UnityEngine;
public class Guard : AbilityHandler
{
    [SerializeField] ReflectProjectile m_reflect = default;
    [SerializeField] Shockwave m_bash = default;
    [SerializeField] NpcAttackHandler m_attackHandler = default;
    [SerializeField] RangeDetector m_attackRadius = default;
    public override List<ActiveAbility> Abilities => new List<ActiveAbility> { m_reflect, m_bash };
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
        if (m_attackRadius.IsInRange(m_attackHandler?.Target))
        {
            m_bash?.Init();
        }
        else
        {
            m_reflect?.Init();
        }
    }
}
