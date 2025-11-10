using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public interface IEffectOverTime<T>
{
    float TimeInterval { get; }
    IEnumerator OnTick(T target);
}
public class RepeatOverTime : EffectModule, IEffectOverTime<BaseEntity>
{
    [SerializeField] protected UnityEvent<BaseEntity> m_triggerPerTick = default;
    [SerializeField] float m_waitSecondsPerTick = default;
    /// <summary>
    /// If value < 1, unlimited uses
    /// </summary>
    [SerializeField] int m_numTicks = default;
    public float TimeInterval => m_waitSecondsPerTick;
    public override void Activate(BaseEntity user)
    {
        base.Activate(user);
        StartCoroutine(OnTick(user));
    }
    public override void Deactivate(BaseEntity target)
    {
        base.Deactivate(target);
        StopAllCoroutines();
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


