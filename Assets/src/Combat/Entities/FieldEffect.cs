using System.Collections.Generic;
using UnityEngine;
//activate effects that affects targets in a defined range
[RequireComponent(typeof(RangeDetector))]  
public class FieldEffect : EffectModule
{
    protected RangeDetector m_rangeRef;
    protected virtual void OnEnable()
    {
        m_rangeRef = GetComponent<RangeDetector>();
    }
    public virtual void TriggerInRange() 
    {
        foreach (var item in m_rangeRef.TargetsInView)
        {
            Activate(item);
        }
    }
}
