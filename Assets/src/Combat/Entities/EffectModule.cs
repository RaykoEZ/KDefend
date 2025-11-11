using UnityEngine;
using UnityEngine.Events;
// Script for generic item/weapon effects
public abstract class EffectModule : MonoBehaviour 
{
    [SerializeField] UnityEvent<BaseEntity> m_onActivate = default;
    [SerializeField] UnityEvent<BaseEntity> m_onDeactivate = default;
    bool m_activated = false;
    protected bool Activated { get => m_activated; }
    public virtual void Activate(BaseEntity target) 
    {
        m_onActivate?.Invoke(target);
        m_activated = true;
    }
    // for reversing/shutting down effects if needed
    public virtual void Deactivate(BaseEntity target) 
    {
        m_onDeactivate?.Invoke(target);
        m_activated = false;
    }
}

