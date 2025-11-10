using UnityEngine;
using UnityEngine.Events;
// continuous are of effect effect
[RequireComponent(typeof(Collider2D))]
public class FieldEffect : EffectModule
{
    [SerializeField] RepeatOverTime m_onTick = default;

    public override void Activate(BaseEntity target)
    {
        base.Activate(target);
    }
    public override void Deactivate(BaseEntity target) 
    {
        base.Deactivate(target);
    }
}
