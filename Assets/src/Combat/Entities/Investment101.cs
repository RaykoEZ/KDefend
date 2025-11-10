using System.Collections;
using UnityEngine;

public class Investment101 : Collectible 
{
    [SerializeField] GainCredit m_gainCredit = default;
    [SerializeField] int m_baseTimeInterval = default;
    int m_currentTimeInterval = 200;
    static Coroutine s_gain;
    // let rank up handle time scaling
    public int CurrentTimeInterval { get => m_currentTimeInterval; set => m_currentTimeInterval = value; }
    void Start()
    {
        m_currentTimeInterval = m_baseTimeInterval;
    }
    // on obtaining, extend max time
    public override void UseItem()
    {
        base.UseItem();
        // reset credit regen routine
        if (s_gain != null) 
        {
            StopAllCoroutines();
        }
        s_gain = StartCoroutine(GainCredit());
    }
    protected IEnumerator GainCredit() 
    {
        while (m_isEffectActive) 
        {
            yield return new WaitForSeconds(CurrentTimeInterval);
            m_gainCredit?.AddToCredit(m_user);
        }
    }
}
