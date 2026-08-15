using System;
using System.Collections;
using UnityEngine;

public class CreditChangeOverTime
{
    Player m_playerRef;
    Coroutine m_change;
    int m_changeOverTime = 0;
    float m_timeInterval = 0f;
    int CurrentCredit => m_playerRef.CurrentStats.Property.Health;
    public CreditChangeOverTime(Player playerRef, int changeOverTime, float timeInterval)
    {
        m_playerRef = playerRef;
        m_changeOverTime = changeOverTime;
        m_timeInterval = timeInterval;
    }
    public void Begin() 
    {
        m_playerRef?.StopCoroutine(m_change);
        bool isRegen = m_timeInterval > 0;
        // create tick call depending on taking heal or damage
        Action tick = isRegen ? () => { m_playerRef?.Heal(m_changeOverTime); } :
            () => { m_playerRef?.TakeDamage(m_changeOverTime); };
        m_change = m_playerRef?.StartCoroutine(CreditActionOverTime(Mathf.Abs(m_timeInterval), tick));       
    }
    public void End() 
    {
        m_playerRef?.StopCoroutine(m_change);
    }
    IEnumerator CreditActionOverTime(float timeInterval, Action tickAction)
    {
        while (CurrentCredit > 0)
        {
            yield return new WaitForSeconds(timeInterval);
            tickAction?.Invoke();
        }
    }
}
