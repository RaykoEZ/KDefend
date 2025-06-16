using System.Collections;
using System;
using UnityEngine;
public abstract class ActiveAbility : MonoBehaviour 
{
    [Range(0, 999)]
    [SerializeField] protected int m_cooldownTime = default;
    [SerializeField] protected int m_hitsToInterrupt = default;
    bool m_onCooldown = false;
    protected int m_disruptCounter = 0;
    protected bool m_isChanneling = false;
    protected delegate void AbilityUpdate();
    // listen to change animation for each skill charge update
    protected event AbilityUpdate OnChannelInterrupt;
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
        if (!m_isChanneling) return;
        m_disruptCounter++;
        if (m_disruptCounter > m_hitsToInterrupt) 
        {
            m_disruptCounter = 0;
            m_isChanneling = false;
            StartCoroutine(Cooldown(m_cooldownTime));
            OnChannelInterrupt?.Invoke();
        }
    }
    protected IEnumerator Channeling(float duration, Action onChannelingFinish) 
    {
        int channelTime = 0;
        while (channelTime < duration)
        {
            channelTime++;
            yield return new WaitForSeconds(1f);
            OnChannelInterval?.Invoke();
            m_isChanneling = true;
            // upon charge complete, incoke effect
        }
        onChannelingFinish?.Invoke();
        m_isChanneling = false;
    }
}