using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
// simple game timer counting up/down in seconds
public class GameTimer : MonoBehaviour 
{
    [SerializeField] TextMeshProUGUI m_secondDisplay = default;
    [SerializeField] UnityEvent<int> m_onTimeElapsed = default;
    bool m_isActive = false;
    int m_secondsElapsed = 0;   
    Coroutine m_timer;
    // start timer from beginning
    public void StartTimer(int initTime = 0, bool countdown = false) 
    {
        if (m_timer != null) return;
        m_secondsElapsed = initTime;
        m_timer = StartCoroutine(UpdateTimer(countdown));
    }
    // Stop timer but keep current time
    public void Pause() 
    {
        if (m_timer == null) return;
        StopCoroutine(m_timer);
        m_timer = null;
    }
    // Start timer from previous pause value
    public void Resume(bool countdown = false) 
    {
        if (m_timer != null) return;
        m_timer = StartCoroutine(UpdateTimer(countdown));
    }
    public void ResetTmer(int initTime = 0) 
    {
        if (m_timer == null) return;
        StopCoroutine(m_timer);
        m_timer = null;
        m_secondsElapsed = initTime;
    }
    IEnumerator UpdateTimer(bool countdown) 
    {
        while (m_isActive) 
        {
            yield return new WaitForSeconds(1f);
            // counting down/up
            m_secondsElapsed += countdown? -1 : 1;
            m_onTimeElapsed?.Invoke(m_secondsElapsed);
        }
    }
}
