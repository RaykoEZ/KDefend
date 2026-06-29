using UnityEngine;
using UnityEngine.Events;
// Script for generic item/weapon effects
public abstract class EffectModule : MonoBehaviour 
{
    [SerializeField] protected bool m_persistAfterUsage = default;
    [SerializeField] protected UnityEvent<BaseEntity> m_onActivate = default;
    [SerializeField] protected UnityEvent<BaseEntity> m_onDeactivate = default;
    bool m_activated = false;
    protected bool Activated { get => m_activated; set => m_activated = value; }
    public virtual void Activate(BaseEntity target) 
    {
        // attach to target
        gameObject.transform.SetParent(target.transform, false);
        m_onActivate?.Invoke(target);
        m_activated = true;
    }
    // for reversing/shutting down effects if needed
    public virtual void Deactivate(BaseEntity target) 
    {
        m_onDeactivate?.Invoke(target);
        m_activated = false;
        if (!m_persistAfterUsage) 
        {
            // when done remove self
            Destroy(gameObject);
        }
    }
}

