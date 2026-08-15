using System;
using System.Collections;
using UnityEngine;
[Serializable]
public class LoadingScreenSequencer 
{
    [SerializeField] Animator m_transition = default;
    public IEnumerator FadeOut() 
    {
        // play transition animaton
        m_transition?.ResetTrigger("transition");
        m_transition?.SetTrigger("transition");
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator FadeIn() 
    {
        m_transition?.ResetTrigger("finish");
        m_transition?.SetTrigger("finish");
        yield return new WaitForSeconds(1.25f);
    }
}
