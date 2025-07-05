using UnityEngine.Playables;
using UnityEngine;
using System.Collections;
public class Deadeye : ActiveAbility
{
    [SerializeField] float m_aimTime = default;
    // for aiming
    [SerializeField] SniperLaser m_aimLaser = default;
    [SerializeField] Enemy m_user = default;
    [SerializeField] PlayableDirector m_aimSequence = default;
    // target when aiming
    BaseEntity m_target;
    bool m_targetAcquired = false;
    public BaseEntity Target { get => m_target; }
    void FixedUpdate()
    {
        if (Target != null && m_channeling == null) 
        {
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
        StartChanneling(m_aimTime, OnShoot, OnAimingUpdate);
    }
    // during aiming mode...
    void OnAimingUpdate(float dt, float timeElapsed)
    {
        if (m_target == null) 
        {
            m_targetAcquired = false;
            return;
        }
        var hit = m_aimLaser.PointTowards(m_target.transform.position);
        if (hit.rigidbody == null) 
        {
            m_targetAcquired = false;
        }
        else
        {
            m_targetAcquired = hit.rigidbody.TryGetComponent(out BaseEntity result) && result == m_target;
        }
    }
    protected override void OnInterrupted() 
    {
        base.OnInterrupted();
    }
    void OnShoot()
    {
        StartCoroutine(Cooldown(m_cooldownTime));
            // line dissipates from target position
            // (ray blinking with sfx)
            // delay
            // sniper shot releases to target position
        StartChanneling(0.15f,
            () =>
            {
                if (m_targetAcquired) 
                {
                    m_user?.UseWeapon();
                }
                m_aimLaser?.Clear();
            });       
    }
}

