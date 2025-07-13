using System.Collections;
using System;
using UnityEngine;
public abstract class ActiveAbility : MonoBehaviour 
{
    [Range(0, 999)]
    [SerializeField] protected int m_cooldownTime = default;
    [SerializeField] protected int m_hitsToEnd = default;
    protected bool m_onCooldown = false;
    protected int m_disruptCounter = 0;
    protected Coroutine m_channeling;
    protected delegate void AbilityUpdate();
    // listen to change animation for each skill charge update
    protected event AbilityUpdate OnChannelInterrupt;
    public void TryUse() 
    {
        if (m_onCooldown) return;
        StartCoroutine(Cooldown(m_cooldownTime));
        Effect_Internal();
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
        if (m_channeling == null) return;
        m_disruptCounter++;
        if (m_disruptCounter > m_hitsToEnd && m_channeling != null) 
        {
            OnInterrupted();
        }
    }
    protected virtual void StartChanneling(float duration, Action onChannelingFinish, Action<float, float> onInterval = null) 
    {
        m_channeling = StartCoroutine(Channeling(duration, onChannelingFinish, onInterval));
    }
    protected virtual void OnInterrupted() 
    {
        StopCoroutine(m_channeling);
        m_channeling = null;
        m_disruptCounter = 0;
        StartCoroutine(Cooldown(m_cooldownTime));
        OnChannelInterrupt?.Invoke();
    }
    // onInterval <float, float> : deltatime, totalTimeElasped
    IEnumerator Channeling(float duration, Action onChannelingFinish, Action<float, float> onInterval = null) 
    {
        float channelTime = 0;
        while (channelTime < duration)
        {
            yield return new WaitForEndOfFrame();
            channelTime += Time.deltaTime;
            onInterval?.Invoke(Time.deltaTime, channelTime);
            // upon charge complete, incoke effect
        }
        onChannelingFinish?.Invoke();
        m_channeling = null;
    }
}