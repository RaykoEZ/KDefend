using Curry.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
// simple game timer counting up/down in seconds
public class GameTimer : MonoBehaviour 
{
    [SerializeField] int m_startTimeValue = default;
    [SerializeField] TextMeshProUGUI m_secondDisplay = default;
    [SerializeField] UnityEvent m_onTimeOut = default;
    [SerializeField] UnityEvent<KDefenderEventContext> m_onTimeElapsed = default;
    [SerializeField] GameSaveSource m_gameState = default;
    int m_secondsElapsed = 10;
    int m_timeExtension = 0;
    Coroutine m_timer;
    public int SecondsElapsed => m_secondsElapsed;
    public int StartTimeValue { get => m_startTimeValue; set => m_startTimeValue = value; }
    public int TimeExtension { get => m_timeExtension; set => m_timeExtension = value; }
    void OnEnable()
    {
        StartTimer();
        KDEventHandler.ListenToGlobal(GameEventTriggerType.TimeUpdate, OnExtendTime);
    }
    void OnDisable()
    {
        Pause();
        KDEventHandler.UnlistenFromGlobal(GameEventTriggerType.TimeUpdate, OnExtendTime);
    }

    public void OnExtendTime(object sender, KDEventInfo args) 
    {
        if (args == null || args.Payload == null) return;
        if (args.Payload.TryGetValue("extend", out object result) && result is int extend)
        {
            TimeExtension += extend;
        }
    }

    // start timer from beginning
    public void StartTimer() 
    {
        if (m_timer != null) return;
        m_secondsElapsed = StartTimeValue + TimeExtension;
        m_secondDisplay.text = StartTimeValue.ToString();
        m_timer = StartCoroutine(UpdateTimer());
    }
    // Stop timer but keep current time
    public void Pause() 
    {
        if (m_timer == null) return;
        StopCoroutine(m_timer);
        m_timer = null;
    }
    // Start timer from previous pause value
    public void Resume() 
    {
        if (m_timer != null) return;
        m_timer = StartCoroutine(UpdateTimer());
    }
    public void ResetTmer() 
    {
        if (m_timer == null) return;
        StopCoroutine(m_timer);
        m_timer = null;
        m_secondsElapsed = StartTimeValue;
    }
    IEnumerator UpdateTimer() 
    {
        while (m_secondsElapsed > 0) 
        {
            yield return new WaitForSeconds(1f);
            // counting down/up
            m_secondsElapsed++;
            m_secondDisplay.text = (StartTimeValue + m_timeExtension - m_secondsElapsed).ToString();
            m_gameState.Current.KDGameState.Timer = m_secondsElapsed;
            if (m_secondsElapsed <= 0) 
            {
                m_onTimeOut?.Invoke();
                yield break;
            }
            else 
            {
                Dictionary<GameEventTriggerType, object> p = new Dictionary<GameEventTriggerType, object>
                {
                    {GameEventTriggerType.TimeUpdate, m_secondsElapsed}
                };
                KDefenderEventContext e = new KDefenderEventContext(m_gameState.Current.KDGameState, p);
                m_onTimeElapsed?.Invoke(e);
            }
        }
    }
}
