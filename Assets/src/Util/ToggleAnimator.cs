using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(Toggle))]
public class ToggleAnimator : MonoBehaviour
{
    Toggle m_toggle;
    Animator m_anim;
    void OnEnable()
    {
        m_toggle = GetComponent<Toggle>();
        m_anim = GetComponent<Animator>();
        SetAnimatorBool(m_toggle.isOn);
    }
    public void SetAnimatorBool(bool isOn) 
    {
        m_anim?.SetBool("isOn", isOn);
    }
    public void OnHoverExit() 
    {
        if (!m_toggle.isOn) 
        {
            m_anim?.SetTrigger("Normal");
        }
    }
}