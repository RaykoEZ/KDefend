using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
// simple game timer counting up/down in seconds
public class GameTimer : MonoBehaviour 
{
    [SerializeField] TextMeshProUGUI m_secondDisplay = default;
    [SerializeField] UnityEvent m_onTimeOut = default;
    [SerializeField] UnityEvent<GameEventContext> m_onTimeElapsed = default;
    [SerializeField] KDefenderStateManager m_gameState = default;
    int m_secondsElapsed = 1;   
    Coroutine m_timer;
    // start timer from beginning
    public void StartTimer(int initTime = 1, bool countdown = false) 
    {
        if (m_timer != null) return;
        m_secondsElapsed = initTime;
        m_secondDisplay.text = initTime.ToString();
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
    public void ResetTmer(int initTime = 1) 
    {
        if (m_timer == null) return;
        StopCoroutine(m_timer);
        m_timer = null;
        m_secondsElapsed = initTime;
    }
    IEnumerator UpdateTimer(bool countdown) 
    {
        while (m_secondsElapsed > 0) 
        {
            yield return new WaitForSeconds(1f);
            // counting down/up
            m_secondsElapsed += countdown? -1 : 1;
            m_secondDisplay.text = m_secondsElapsed.ToString();
            if (m_secondsElapsed == 0) 
            {
                m_onTimeOut?.Invoke();
                yield break;
            }
            else 
            {
                Dictionary<GameEventTriggerType, object> p = new Dictionary<GameEventTriggerType, object>
                {
                    {GameEventTriggerType.Time, m_secondsElapsed}
                };
                GameEventContext e = new GameEventContext(m_gameState.Current, p);
                m_onTimeElapsed?.Invoke(e);
            }
        }
    }
}
