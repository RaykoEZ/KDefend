using Curry.Explore;
using System;
using System.Collections;
using UnityEngine;

public class ScreenFade : HideableUI
{
    [SerializeField] AnimationClip m_fadeInClip = default;
    bool m_inProgress = false;
    public void StartFade(IEnumerator doWhenFade, float holdFadeLength = 0.5f)
    {
        if (!m_inProgress) 
        {
            m_inProgress = true;
            StartCoroutine(Fade_Internal(doWhenFade, holdFadeLength));
        }
    }
    IEnumerator Fade_Internal(IEnumerator doWhenFade, float holdLength)
    {
        Show();
        yield return new WaitForSeconds(m_fadeInClip.length);
        yield return StartCoroutine(doWhenFade);
        yield return new WaitForSeconds(holdLength);
        Hide();
        m_inProgress = false;
    }
}
