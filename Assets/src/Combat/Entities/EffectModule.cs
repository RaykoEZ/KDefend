using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public interface IEffectOverTime<T>
{
    float TimeInterval { get; }
    IEnumerator OnTick(T target);
}
// Script for generic item/weapon effects
public abstract class EffectModule : MonoBehaviour 
{
    public abstract void Activate(BaseEntity target);
    // for reversing/shutting down effects if needed
    public virtual void Deactivate(BaseEntity target) { } 
}
public class Heal : EffectModule
{
    [SerializeField] protected int m_healAmount = default;
    public override void Activate(BaseEntity target)
    {
        target?.Heal(m_healAmount);
    }
}
public class TakeDamage : EffectModule
{
    [SerializeField] protected int m_damage = default;
    public override void Activate(BaseEntity target)
    {
        target?.TakeDamage(m_damage);
    }
}
public class RepeatOverTime : EffectModule, IEffectOverTime<BaseEntity>
{
    [SerializeField] protected UnityEvent<BaseEntity> m_triggerPerTick = default;
    [SerializeField] float m_waitSecondsPerTick = default;
    [SerializeField] int m_numTicks = default;
    public float TimeInterval => m_waitSecondsPerTick;
    public override void Activate(BaseEntity user)
    {
        StartCoroutine(OnTick(user));
    }
    public IEnumerator OnTick(BaseEntity user) 
    {
        for (int i = 0; i < m_numTicks; i++)
        {
            m_triggerPerTick?.Invoke(user);
            yield return new WaitForSeconds(m_waitSecondsPerTick);
            i++;
        }
    }
}


