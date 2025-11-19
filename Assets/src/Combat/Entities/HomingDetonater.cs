using System.Collections;
using UnityEngine;
using UnityEngine.Events;
// Script for a guided object that triggers a detonation effect
public class HomingDetonater : MonoBehaviour
{
    [SerializeField] RangeDetector m_targeting = default;
    [SerializeField] RangeDetector m_blastRadius = default;
    [SerializeField] NpcMovement m_movement = default;
    [SerializeField] UnityEvent<BaseEntity> m_onDetonate = default;
    bool m_isActive = false;
    BaseEntity m_targetRef;
    Coroutine m_detonating;
    public void InitAttack()
    {
        var targets = m_targeting.TargetsInView;
        if (targets.Count > 0) 
        { 
            int i = Random.Range(0, targets.Count - 1);
            Debug.Log(targets[i].name);
            Chase(targets[i]);
        }
    }
    public void Chase(BaseEntity target) 
    {
        m_isActive = true;
        m_targetRef = target;
        m_movement?.Init(m_targetRef);
        m_movement?.StartMoving();
    }
    // Finds targets from blast radius and deal with the effects
    IEnumerator Detonate() 
    {
        if (!m_isActive) yield break;
        m_isActive = false;
        // delay detonation a bit
        yield return new WaitForSeconds(0.5f);
        var targets = m_blastRadius.TargetsInView;
        foreach (var item in targets)
        {
            m_onDetonate?.Invoke(item);
        }
    }
    public void Detonate(BaseEntity hit)
    {
        if (hit == m_targetRef && m_detonating == null) 
        {
            m_detonating = StartCoroutine(Detonate());
        }
    }
}