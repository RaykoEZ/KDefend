using UnityEngine.Playables;
using UnityEngine;
using System.Collections;
// Enter Aim Mode to attack, enemy does not auto attack 
public class Deadeye : ActiveAbility
{
    [SerializeField] float m_aimTime = default;
    // for aiming
    [SerializeField] Laser m_aimLaser = default;
    [SerializeField] AttackHandler m_user = default;
    // target when aiming
    BaseEntity m_target;
    bool m_targetAcquired = false;
    public BaseEntity Target { get => m_target; }
    void FixedUpdate()
    {
        if (m_channeling != null)
        {
            OnAimingUpdate();
        }
        else if (Target != null)
        {
            
            // start aiming
            TryUse();
        }
    }
    public void LockOn(BaseEntity target)
    {
        m_target = target;
    }
    public void ResetTarget()
    {
        m_target = null;
    }
    protected override void Effect_Internal()
    {
        StartChanneling(m_aimTime, OnShoot);
    }
    // during aiming mode...
    void OnAimingUpdate()
    {
        if (m_target == null) 
        {
            m_aimLaser?.Clear();
            OnInterrupted();
            m_targetAcquired = false;
            return;
        }
        Vector3 dir = m_target.transform.position - transform.position;
        var hit = m_aimLaser.PointTowardsDirection(dir.normalized);
        if (hit.rigidbody == null) 
        {
            m_targetAcquired = false;
        }
        else
        {
            m_targetAcquired = hit.rigidbody.TryGetComponent(out BaseEntity result) && result == m_target;
        }
    }
    void OnShoot()
    {
        m_onCooldown = StartCoroutine(Cooldown(m_cooldownTime));
            // line dissipates from target position
            // (ray blinking with sfx)
            // delay
            // sniper shot releases to target position
        StartChanneling(0.2f, ShootOrWait);       
    }
    void ShootOrWait() 
    {
        if (m_targetAcquired)
        {
            m_user?.UseWeapon();
        }
        m_aimLaser?.Clear();
    }
}

