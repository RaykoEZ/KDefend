using System.Collections;
using UnityEngine;

// Enter Aim Mode to attack, enemy does not auto attack
public class NpcAimingWeapon : UseWeapon
{
    [SerializeField] float m_aimTime = default;
    // for aiming
    [SerializeField] Laser m_aimLaser = default;
    bool m_targetAcquired = false;
    bool m_aiming = false;
    public override bool CanUse()
    {
        return base.CanUse() && Target != null && !m_aiming;
    }
    void FixedUpdate()
    {
        if (m_channeling != null)
        {
            OnAimingUpdate();
        }
        else if (CanUse())
        {
            Debug.Log("New Aiming");
            // start aiming
            Init();
        }
    }
    public override void ResetTarget()
    {
        base.ResetTarget();
        m_aimLaser?.Clear();
        m_aiming = false;
    }
    protected override void Effect_Internal()
    {
        m_aiming = true;
        StartChanneling(m_aimTime, Attack);
    }
    // during aiming mode...
    void OnAimingUpdate()
    {
        if (Target == null) 
        {
            m_aimLaser?.Clear();
            OnInterrupted();
            m_targetAcquired = false;
            return;
        }
        (m_attackHandler as NpcAttackHandler)?.UpdateTarget(Target);
        Vector3 dir = AimDirection();
        var hit = m_aimLaser.PointTowardsDirection(dir.normalized, passThroughTarget: false);
        // collision check for target or obstacles
        m_targetAcquired = 
            hit.rigidbody == null ? 
            false : 
            hit.rigidbody.TryGetComponent(out BaseEntity result) && result == Target;
    }
    protected override void OnAttack()
    {
        // line dissipates from target position
        // (ray blinking with sfx)
        // delay
        // sniper shot releases to target position
        (m_attackHandler as NpcAttackHandler)?.UpdateTarget(Target);
        StartChanneling(0.25f, ShootOrWait);       
    }
    protected override Vector2 AimDirection()
    {
        // set target for attack
        return m_attackHandler.GetAimDirectionNormalized();
    }
    void ShootOrWait() 
    {
        if (m_targetAcquired)
        {
            // set target for attack
            m_attackHandler.UseWeaponOneShot(m_weaponRotation, AimDirection());
        }
        m_onCooldown = StartCoroutine(Cooldown(CooldownTime));
        // reset skill states
        m_aimLaser?.Clear();
        m_aiming = false;
    }
}

