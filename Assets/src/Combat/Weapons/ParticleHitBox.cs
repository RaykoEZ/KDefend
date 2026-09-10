using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(ParticleSystem))]
public class ParticleHitBox : MonoBehaviour 
{
    // time interval between each collision effect trigger
    [SerializeField] float m_effectTriggerInterval = default;
    [SerializeField] UnityEvent<BaseEntity> m_onHit = default;
    [SerializeField] UnityEvent m_onAnyHit = default;

    IEnumerator OnParticleCollision(GameObject other)
    {
        m_onAnyHit?.Invoke();
        // check for parent entity script to affect the hit entity
        if (other.transform.TryGetComponent(out BaseEntity hit))
        {
            m_onHit?.Invoke(hit);
        }
        yield return new WaitForSeconds(m_effectTriggerInterval);       
    }

}
