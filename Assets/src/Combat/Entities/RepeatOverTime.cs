using System.Collections;
using UnityEngine;
public interface IEffectOverTime<T>
{
    float TimeInterval { get; }
    IEnumerator OnTick(T target);
}
public class RepeatOverTime : EffectModule, IEffectOverTime<BaseEntity>
{
    [SerializeField] float m_waitSecondsPerTick = default;
    /// <summary>
    /// If value < 1, unlimited uses
    /// </summary>
    [SerializeField] int m_numTicks = default;
    public float TimeInterval => m_waitSecondsPerTick;
    /// <summary>
    /// Start Effect cycle
    /// </summary>
    /// <param name="target"></param> User of this effect module.
    public override void Activate(BaseEntity target)
    {
        m_activated = true;
        StartCoroutine(OnTick(target));
    }
    public override void Deactivate(BaseEntity target)
    {
        base.Deactivate(target);
        StopAllCoroutines();
    }
    public virtual IEnumerator OnTick(BaseEntity target)
    {
        int i = 0;
        while (Activated)
        {
            // check if we repeat, tick limit <= 0 means unlimited repeats
            if (m_numTicks >= 0 && i >= m_numTicks) yield break;
            m_onActivate?.Invoke(target);
            yield return new WaitForSeconds(m_waitSecondsPerTick);
            i++;          
        }
    }
}


