using System.Collections;
using UnityEngine.Events;
using UnityEngine;
public interface IEffectOverTime<T>
{
    float TimeInterval { get; }
    IEnumerator OnTick(T target);
}
public class RepeatOverTime : EffectModule, IEffectOverTime<BaseEntity>
{
    [SerializeField] protected UnityEvent<BaseEntity> m_onTick = default;
    [SerializeField] protected float m_waitSecondsPerTick = default;
    /// <summary>
    /// If value < 1, unlimited uses
    /// </summary>
    [SerializeField] protected int m_numTicks = default;
    public float TimeInterval => m_waitSecondsPerTick;
    /// <summary>
    /// Start Effect cycle
    /// </summary>
    /// <param name="target"></param> User of this effect module.
    public override void Activate(BaseEntity target)
    {
        base.Activate(target);
        StartCoroutine(OnTick(target));
    }
    public override void Deactivate(BaseEntity target)
    {
        StopAllCoroutines();
        base.Deactivate(target);
    }
    public virtual IEnumerator OnTick(BaseEntity target)
    {
        int i = 0;
        while (Activated)
        {
            // check if we repeat, tick limit <= 0 means unlimited repeats
            if (m_numTicks >= 0 && i >= m_numTicks) 
            {
                Activated = false;
            }
            m_onTick?.Invoke(target);
            yield return new WaitForSeconds(m_waitSecondsPerTick);
            i++;          
        }
        Deactivate(target);
    }
}


