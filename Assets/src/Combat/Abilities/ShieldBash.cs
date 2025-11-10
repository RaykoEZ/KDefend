using UnityEngine;

public class ShieldBash : ActiveAbility
{
    [SerializeField] float m_pushPower = default;
    [SerializeField] int m_damage = default;
    private BaseEntity m_target;
    public BaseEntity Target { get => m_target; set => m_target = value; }
    protected override void Effect_Internal()
    {
        Bash(Target);
    }
    // Knockback player
    void Bash(BaseEntity target)
    {
        if (Target == null) return;
        Vector2 pushDir = target.transform.position - transform.position;
        target.TakeDamage(m_damage);
        if (target is IPushable push) 
        {
            push.Push(pushDir.normalized, m_pushPower);
        }
    }
}
