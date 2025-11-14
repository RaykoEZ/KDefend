using System.Collections;
using UnityEngine;
using UnityEngine.Events;
// Script for a guided object that triggers a detonation effect
public class HomingDetonater : MonoBehaviour, IHitsEntity
{
    [Range(0f, 30f)]
    [SerializeField] float m_secondsBeforeDetonation = default;
    [SerializeField] RangeDetector m_blastRadius = default;
    [SerializeField] NpcMovement m_movement = default;
    [SerializeField] UnityEvent<BaseEntity> m_onDetonate = default;
    bool m_isActive = false;
    BaseEntity m_targetRef;
    public void Init()
    {
        var targets = m_blastRadius.TargetsInView;
        if (targets.Count > 0) 
        { 
            int i = Random.Range(0, targets.Count - 1);
            Chase(targets[i]);
        }
    }
    public void Chase(BaseEntity target) 
    {
        m_isActive = true;
        m_targetRef = target;
        m_movement?.Init(m_targetRef);
        StartCoroutine(DetonateTimer());
    }
    // Finds targets from blast radius and deal with the effects
    public IEnumerator Detonate() 
    {
        if (!m_isActive) yield break;
        m_isActive = false;
        // delay detonation a bit
        yield return new WaitForSeconds(0.2f);
        var targets = m_blastRadius.TargetsInView;
        foreach (var item in targets)
        {
            m_onDetonate?.Invoke(item);
        }
    }
    IEnumerator DetonateTimer() 
    { 
        yield return new WaitForSeconds(m_secondsBeforeDetonation);
        yield return Detonate();
    }
    public void OnHit<T>(T hit) where T : BaseEntity
    {
        if (hit == m_targetRef) 
        {
            StartCoroutine(Detonate());
        }
    }
}