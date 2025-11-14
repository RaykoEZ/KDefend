using UnityEngine;

public class Knockback : EffectModule 
{
    [SerializeField] float m_knockbackPower = default;
    public override void Activate(BaseEntity target)
    {
        base.Activate(target);
        Vector2 pushDir = target.transform.position - transform.position;
        if (target is IPushable push)
        {
            push.Push(pushDir.normalized, m_knockbackPower);
        }
    }
}
