using UnityEngine;
// Hides self for some time
public class Stealth : ActiveAbility
{
    [SerializeField] Animator m_anim = default;
    [SerializeField] float m_duration = default;
    protected override void Effect_Internal()
    {
        if (m_channeling != null) return;
        OnChannelInterrupt += DisableStealth;
        m_anim?.SetBool("hide", true);
        StartChanneling(m_duration, DisableStealth);
    }
    void DisableStealth() 
    {
        OnChannelInterrupt -= DisableStealth;
        m_anim?.SetBool("hide", false);
    }
}