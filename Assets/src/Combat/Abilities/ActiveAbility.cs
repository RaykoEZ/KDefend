using System.Collections;
using System;
using UnityEngine;
public abstract class ActiveAbility : MonoBehaviour 
{
    [Range(0, 999)]
    [SerializeField] protected int m_cooldownTime = default;
    bool m_onCooldown = false;
    protected bool m_isChanneling = false;
    protected delegate void AbilityUpdate();
    // listen to change animation for each skill charge update
    protected event AbilityUpdate OnChannelInterval;
    public bool TryUse() 
    {
        if (m_onCooldown) return false;
        StartCoroutine(Cooldown(m_cooldownTime));
        Effect_Internal();
        return true;
    }
    protected abstract void Effect_Internal();
    protected IEnumerator Cooldown(float duration) 
    {
        m_onCooldown = true;
        yield return new WaitForSeconds(duration);
        m_onCooldown = false;
    }
    public virtual void InterruptChanneling()
    {
        m_isChanneling = false;
        StartCoroutine(Cooldown(m_cooldownTime));
    }
    protected IEnumerator Channeling(float duration, Action onChannelingFinish) 
    {
        m_isChanneling = true;
        int channelTime = 0;
        while (m_isChanneling)
        {
            yield return new WaitForSeconds(1f);
            channelTime++;
            OnChannelInterval?.Invoke();
            // upon charge complete, incoke effect
            if (channelTime >= duration) 
            {
                onChannelingFinish?.Invoke();
                m_isChanneling = false;
            }
        }
    }
}
