using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
// effects usable via triggers or commands, has charging/channeling feature
public abstract class ActiveAbility : MonoBehaviour 
{
    [SerializeField] protected bool m_activateOnInit = default;
    [SerializeField] protected bool m_playSequenceOnInit = default;
    [Range(0f, 999f)]
    [SerializeField] protected float m_cooldownTime = default;
    [SerializeField] protected int m_hitsToEnd = default;
    [SerializeField] protected PlayableDirector m_activationSequence = default;
    [SerializeField] protected UnityEvent<BaseEntity> m_activateEffects = default;
    protected Coroutine m_onCooldown;
    protected int m_disruptCounter = 0;
    protected Coroutine m_channeling;
    private BaseEntity m_target;
    protected delegate void AbilityUpdate();
    // listen to change animation for each skill charge update
    protected event AbilityUpdate OnChannelInterrupt;
    public BaseEntity Target { 
        get => m_target; 
        set => m_target = value; }
    public virtual float CooldownTime { get => m_cooldownTime; set => m_cooldownTime = value; }
    public virtual void Init() 
    {
        if (!CanUse()) return;
        if (m_activationSequence != null && m_playSequenceOnInit) 
        {
            GameUtil.PlaySequence(m_activationSequence);
        }
        if (m_activateOnInit) 
        {
            Activate();
        }
    }
    public virtual bool CanUse() => (m_onCooldown == null && m_channeling == null);
    protected virtual void Effect_Internal() { }
    public virtual void Activate(BaseEntity target)
    {
        if (target == null) return;
        Target = target;
        HandleAbilityStates();
        Effect_Internal();
        m_activateEffects?.Invoke(Target);
    }
    // no need to target
    public virtual void Activate(Vector2 direction)
    {
        HandleAbilityStates();
        Effect_Internal();     
        m_activateEffects?.Invoke(Target);     
    }
    public virtual void Activate()
    {
        HandleAbilityStates();
        Effect_Internal();
        m_activateEffects?.Invoke(Target);
    }
    public virtual void ResetTarget()
    {
        Target = null;
    }
    void HandleAbilityStates() 
    {
        m_onCooldown = StartCoroutine(Cooldown(m_cooldownTime));
        if (m_channeling != null)
        {
            StopCoroutine(m_channeling);
            m_channeling = null;
        }
    }
    protected IEnumerator Cooldown(float duration) 
    {
        yield return new WaitForSeconds(duration);
        m_onCooldown = null;
    }
    public virtual void InterruptChanneling()
    {
        if (m_channeling == null || m_hitsToEnd < 0) return;
        m_disruptCounter++;
        if (m_disruptCounter > m_hitsToEnd && m_channeling != null) 
        {
            OnInterrupted();
        }
    }
    protected virtual void StartChanneling(float duration, Action onChannelingFinish = null, Action<float, float> onInterval = null) 
    {
        m_channeling = StartCoroutine(Channeling(duration, onChannelingFinish, onInterval));
    }
    protected virtual void OnInterrupted() 
    {
        StopCoroutine(m_channeling);
        m_channeling = null;
        m_disruptCounter = 0;
        m_onCooldown = StartCoroutine(Cooldown(m_cooldownTime));
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