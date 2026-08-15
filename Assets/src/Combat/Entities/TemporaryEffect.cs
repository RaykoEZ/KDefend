using System.Collections;
using UnityEngine;

public class TemporaryEffect : EffectModule
{
    [SerializeField] protected EffectModule m_toTrigger = default;
    [SerializeField] protected float m_duration = default;
    public override void Activate(BaseEntity target)
    {
        base.Activate(target);
        // trigger effect once
        m_toTrigger?.Activate(target);
        StartCoroutine(OnTick(target));
    }
    public IEnumerator OnTick(BaseEntity target)
    {
        float t = 0f;
        while (t < m_duration)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }
        // reverse/shutdown effect here
        m_toTrigger?.Deactivate(target);
    }
}

