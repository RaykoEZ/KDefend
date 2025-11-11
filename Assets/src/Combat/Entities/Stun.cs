using UnityEngine;

public class Stun : EffectModule
{
    [Range(0f, 60f)]
    [SerializeField] float m_duration = default;
    [Range(0.01f, 1f)]
    [SerializeField] float m_stunChance = default;
    public override void Activate(BaseEntity target)
    {
        float rand = Random.Range(0f, 1f);
        // roll for stun chance
        if (rand < m_stunChance) 
        {
            base.Activate(target);
            target?.GetComponent<Stunnable>()?.Stun(m_duration);
        }
    }
}
