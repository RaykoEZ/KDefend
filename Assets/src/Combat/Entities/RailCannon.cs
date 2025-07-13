using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Laser))]
public class RailCannon : BaseProjectile 
{
    Laser m_laserRef;
    BaseEntity m_targetRef;
    Coroutine m_hit;
    void Start()
    {
        m_laserRef = GetComponent<Laser>();
    }
    void FixedUpdate()
    {
        if (m_hit == null && m_targetRef != null) 
        {
            m_hit = StartCoroutine(HitTarget(m_targetRef));
        }
    }
    IEnumerator HitTarget(BaseEntity target)
    {
        int numAttackPerTick = WeaponProperty.AttackPerCycle;
        // trigger hit sequence
        while (target != null) 
        {
            for (int i = 0; i < numAttackPerTick; i++)
            {
                OnHit(target);
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(WeaponProperty.DelayPerAttack);
        }
        m_hit = null;
    }
    protected override void OnTriggerEnter2D(Collider2D c)
    {
    }
    protected override IEnumerator InProgress()
    {
        RaycastHit2D hit;
        while (m_isFlying) 
        {
            // detect hit target in this direction
            hit = m_laserRef.PointTowardsDirection(m_currentDirection);
            // deal with hit target if it hits a player
            if (hit.rigidbody != null && 
                hit.rigidbody.transform.TryGetComponent(out BaseEntity result)) 
            {
                m_targetRef = result;
            }
            else 
            {
                m_targetRef = null;
            }
            yield return new WaitForSeconds(0.1f);
            m_lifeTimer += Time.fixedDeltaTime;
            if (m_lifeTimer >= WeaponProperty.Life)
            {
                m_isFlying = false;
            }
        }
        // stop laser loop & dealing damage
        EndProjectile();
        StopCoroutine(m_hit);
        m_hit = null;
        m_targetRef = null;
    }
}