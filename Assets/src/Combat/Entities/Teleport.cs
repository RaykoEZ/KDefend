using UnityEngine;

public class Teleport : EffectModule 
{
    [SerializeField] Transform m_destination = default;
    public override void Activate(BaseEntity target)
    {
        base.Activate(target);
        target.transform.position = m_destination.position;
    }
}
