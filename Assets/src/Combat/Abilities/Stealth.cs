using UnityEngine;
// Hides self for some time
public class Stealth : ActiveAbility
{
    [SerializeField] Animator m_anim = default;
    [SerializeField] float m_duration = default;
    protected override void Effect_Internal()
    {
        if (m_isChanneling) return;
        m_anim?.SetBool("hide", true);
        StartCoroutine(Channeling(m_duration, DisableStealth));
    }
    public void DisableStealth() 
    {
        m_isChanneling = false;
        m_anim?.SetBool("hide", false);
    }
}