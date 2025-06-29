using UnityEngine;

public class ShieldBash : ActiveAbility
{
    [SerializeField] float m_pushPower = default;
    [SerializeField] int m_damage = default;
    Enemy Self => GetComponent<Enemy>();
    protected override void Effect_Internal()
    {
        BaseCharacter target = Self?.CurrentTarget as BaseCharacter;
        if (target == null)
        {
            return;
        }
        Bash(target);
    }
    // Knockback player
    void Bash(BaseCharacter target)
    {
        Vector2 pushDir = target.transform.position - transform.position;
        target.TakeDamage(m_damage);
        target.Push(pushDir.normalized, m_pushPower);
    }
}
