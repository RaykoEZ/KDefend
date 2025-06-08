using System.Collections;
using UnityEngine;

public abstract class ActiveAbility : MonoBehaviour 
{
    [SerializeField] protected float m_cooldown = default;
    bool m_onCooldown = false;
    public bool TryUse() 
    {
        if (m_onCooldown) return false;
        StartCoroutine(Cooldown());
        Effect_Internal();
        return true;
    }
    protected abstract void Effect_Internal();
    IEnumerator Cooldown() 
    {
        m_onCooldown = true;
        yield return new WaitForSeconds(m_cooldown);
        m_onCooldown = false;
    }
}
