using System.Collections.Generic;
using UnityEngine;
//enemy agent behaviour
[RequireComponent(typeof(BaseCharacter))]
public abstract class AbilityHandler : MonoBehaviour 
{
    protected BaseCharacter Self => GetComponent<BaseCharacter>();
    public abstract List<ActiveAbility> Abilities { get; }
    protected void Start()
    {
        StartupEffects();
    }
    protected void OnDisable()
    {
        DefeatEffect();
    }
    // trigger effect when defeated
    protected virtual void DefeatEffect() 
    {  
    }
    // effects to activate upon instantiation
    protected virtual void StartupEffects() 
    {    
    }
    // interrupt channeling when taking a hit
    public virtual void OnTakeHit() 
    {
        InterruptChanneling();
    }
    protected virtual void InterruptChanneling()
    {
        foreach (var item in Abilities)
        {
            item?.InterruptChanneling();
        }
    }
}
