using System.Collections;
using UnityEngine;

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

