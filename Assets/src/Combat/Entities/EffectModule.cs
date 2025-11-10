using System.Collections;
using UnityEngine;
using UnityEngine.Events;
// Script for generic item/weapon effects
public abstract class EffectModule : MonoBehaviour 
{
    [SerializeField] UnityEvent<BaseEntity> m_onActivate = default;
    [SerializeField] UnityEvent<BaseEntity> m_onDeactivate = default;
    public virtual void Activate(BaseEntity target) 
    {
        m_onActivate?.Invoke(target);
    }
    // for reversing/shutting down effects if needed
    public virtual void Deactivate(BaseEntity target) 
    {
        m_onDeactivate?.Invoke(target);
    }
}

