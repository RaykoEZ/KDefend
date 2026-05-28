using System;
using UnityEngine;
// use weapon to attack
public class UseWeapon : ActiveAbility
{
    [Range(1, 18)]
    // number of attack per wave
    [SerializeField] protected int m_numAttacksPerCycle = default;
    [SerializeField] protected AttackHandler m_attackHandler = default;
    [SerializeField] protected BaseWeapon m_weaponRotation = default;
    public override float CooldownTime { get => Mathf.Min(m_weaponRotation.WeaponProperty.DelayPerCycle, m_cooldownTime); set => m_cooldownTime = value; }
    protected override void Effect_Internal()
    {
        if (m_weaponRotation == null) return;
        PrepareAttack();
    }
    public void Attack()
    {
        if (m_weaponRotation == null) return;
        // for each strike angle interval, attack with weapon from rotation list
        for (int i = 0; i < m_numAttacksPerCycle; i++)
        {
            OnAttack();
            PostAttack();
        }
    }
    protected virtual Vector2 AimDirectionNormalized()
    {    
        return Target != null? 
            (Target.transform.position - transform.position).normalized :
            m_attackHandler.GetAimDirectionNormalized();
    }
    // called before all attack actions, when finished, invokes attack when finished
    protected virtual void PrepareAttack() 
    {
        Attack();
    }
    protected virtual void OnAttack() 
    {
        Vector3 dir = AimDirectionNormalized();
        m_attackHandler.UseWeaponOneShot(m_weaponRotation, dir);
    }
    protected virtual void PostAttack() 
    {
    }
}
