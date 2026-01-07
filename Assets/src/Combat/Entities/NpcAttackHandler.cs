using System.Collections;
using UnityEngine;

public class NpcAttackHandler : AttackHandler 
{
    [SerializeField] bool m_autoAttack = default;
    [SerializeField] protected RangeDetector m_targeting = default;
    private BaseEntity m_target;
    Coroutine m_attack;
    public bool AutoAttack { get => m_autoAttack; set => m_autoAttack = value; }
    public BaseEntity Target { get => m_target;}
    public override Vector2 GetAimDirectionNormalized()
    {
        return m_target == null ? Vector2.zero : (m_target.transform.position - transform.position).normalized;
    }
    public void ChangeTarget(BaseEntity newTarget) 
    { 
        if (newTarget == null) return;
        m_target = newTarget;
    }
    protected virtual void FixedUpdate()
    {
        if (m_target != null && (m_autoAttack && m_attack == null && m_targeting.IsInRange(m_target)))
        {
            UseWeapon();
        }
        else
        {
            m_keepFiring = false;
        }
    }
    public void SetTargetDetectAll(bool detectAll) 
    {
        m_targeting.CurrentlyDetectAll = detectAll;
    }
    public void UpdateTarget(BaseEntity newTarget)
    {
        if (newTarget == null) return;
        m_target = newTarget;
    }
    public void ResetTarget()
    {
        m_target = null;
    }
    public override void UseWeapon()
    {
        if (m_currentWeapons.Count == 0) return;
        if (m_attack == null)
        {
            m_keepFiring = true;
            m_attack = StartCoroutine(AttackCycle_Sequence());
        }
    }   
    // make attack pattern sequential vs silmultaneous
    IEnumerator AttackCycle_Sequence()
    {
        while (m_keepFiring)
        {
            for (int i = 0; i < m_currentWeapons.Count; i++)
            {
                // unleash one instance of a weapon's attack, include its recovery frames
                yield return Attack_Internal(m_currentWeapons[i], GetAimDirectionNormalized());
            }
            // atop attacking loop if we don't auto fire
            if (!m_autoAttack)
            {
                m_keepFiring = false;
            }
        }
        m_attack = null;
    }

}
