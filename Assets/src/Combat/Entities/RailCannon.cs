using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Laser))]
public class RailCannon : BaseProjectile 
{
    Laser m_laserRef;
    Coroutine m_hit;
    RaycastHit2D m_hitResult;
    void OnEnable()
    {
        m_laserRef = GetComponent<Laser>();
    }
    // register targets hit by the beam
    IEnumerator HitCheck()
    {
        while (m_isFlying) 
        {
            // trigger hit sequence
            if (m_hitResult && m_hitResult.rigidbody != null &&
                m_hitResult.rigidbody.TryGetComponent(out BaseEntity result)) 
            {
                OnHit(result);
            }
            yield return new WaitForSeconds(WeaponProperty.DelayPerAttack);
        }
        m_hit = null;
    }
    public override void OnHit<T>(T hit)
    {
        Hit_Internal(hit);
    }
    protected override IEnumerator InProgress()
    {
        // delay before firing, play animation cue for lockon before shooting
        m_beforeAttack?.Invoke();
        yield return new WaitForSeconds(m_delayBeforeAttack);
        // start hit detect frame
        m_hit = StartCoroutine(HitCheck());
        while (m_isFlying) 
        {
            // make hit box in this direction
            m_hitResult = m_laserRef.PointTowardsDirection(transform.parent.position, m_currentDirection);
            yield return new WaitForEndOfFrame();
            m_lifeTimer += Time.deltaTime;
            if (m_lifeTimer >= WeaponProperty.Life)
            {
                // stop laser loop & dealing damage
                m_isFlying = false;
            }
        }
        // stop laser loop & dealing damage
        EndProjectile();
    }
    protected override void EndProjectile()
    {
        StopCoroutine(m_inProgress);
        m_inProgress = null;
        m_isFlying = false;
        m_lifeTimer = 0f;
        m_laserRef?.Clear();
        StopCoroutine(m_hit);
        m_hit = null;
        Destroy(gameObject);
    }
}