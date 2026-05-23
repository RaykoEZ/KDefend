using System;
using UnityEngine;
// Enter Aim Mode to attack, enemy does not auto attack
public class NpcAimingWeapon : UseWeapon
{
    [Serializable]
    protected struct AimOverride 
    {
        public bool OverrideAimUpdate;
        // force all cases to set to override value
        // if false, value is used as default behaviour
        public bool OverrideAttackDirection;
        public Vector3 AttackDirection;
    };
    [SerializeField] protected float m_aimTime = default;
    [SerializeField] protected AimOverride m_aimOverride = default;
    // for aiming
    [SerializeField] protected Laser m_aimLaser = default;
    protected bool m_targetAcquired = false;
    protected bool m_aiming = false;
    public override bool CanUse()
    {
        return base.CanUse() && !m_aiming && Target != null;
    }
    void FixedUpdate()
    {
        if (m_channeling != null)
        {
            OnAimingUpdate();
        }
        else if (CanUse())
        {
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
    protected override void PrepareAttack()
    {
        m_aiming = true;
        StartChanneling(m_aimTime, Attack);
    }
    // during aiming mode...update target positions & status
    void OnAimingUpdate()
    {
        (m_attackHandler as NpcAttackHandler)?.UpdateTarget(Target);
        if (m_aimOverride.OverrideAimUpdate) 
        {
            return;
        }
        Vector3 dir = AimDirectionNormalized();
        var hit = m_aimLaser.PointTowardsDirection(dir, passThroughTarget: false);
        // collision check for target or obstacles
        m_targetAcquired =
        hit.rigidbody == null ?
        false :
        hit.rigidbody.TryGetComponent(out BaseEntity result) && result == Target;
        if (!m_targetAcquired) 
        {
            m_aimLaser?.Clear();
        }
    }
    protected override void OnAttack()
    {
        // allow firing when we ignore targeting an entity
        if (m_aimOverride.OverrideAimUpdate) 
        {
            m_targetAcquired = true;
        }
        else 
        {
            (m_attackHandler as NpcAttackHandler)?.UpdateTarget(Target);
        }
        // line dissipates from target position
        // (ray blinking with sfx)
        // delay
        // sniper shot releases to target position
        ShootOrWait();       
    }
    protected override Vector2 AimDirectionNormalized()
    {
        // override all cases if forced to
        if (m_aimOverride.OverrideAttackDirection) return m_aimOverride.AttackDirection.normalized;
        // get result
        var result = base.AimDirectionNormalized();
        // set target for attack, override to static direction if no target is found (direction is zero)
        return result == Vector2.zero? m_aimOverride.AttackDirection.normalized : base.AimDirectionNormalized();
    }
    // trigger this to shoot after aiming finishes
    void ShootOrWait() 
    {
        if (m_targetAcquired)
        {
            m_activationSequence?.Play();
        }
        m_onCooldown = StartCoroutine(Cooldown(CooldownTime));
        // reset skill states
        m_aimLaser?.Clear();
        m_aiming = false;
    }
    public void Shoot() 
    {
        // set target for attack
        m_attackHandler.UseWeaponOneShot(m_weaponRotation, AimDirectionNormalized());
    }
}