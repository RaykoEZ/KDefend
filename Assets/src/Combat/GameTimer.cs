using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour 
{
    [SerializeField] TextMeshProUGUI m_secondDisplay = default;
    [SerializeField] UnityEvent<int> m_onTimeElapsed = default;
    bool m_isActive = false;
    int m_secondsElapsed = 0;   
    Coroutine m_timer;
    IEnumerator UpdateTimer() 
    {
        while (m_isActive) 
        {
            yield return new WaitForSeconds(1f);
            ++m_secondsElapsed;
            
        }
    }
}
