using UnityEngine.Events;
using UnityEngine;
using TMPro;
using Curry.Events;
using System.Collections;
using System;
// Handles player reward and payment
public class PlayerCreditManager : MonoBehaviour
{
    [SerializeField] Player m_player = default;
    [SerializeField] TextMeshProUGUI m_display = default;
    [SerializeField] TextMeshProUGUI m_animate = default;
    [SerializeField] UnityEvent m_onCreditUpdate = default;
    Coroutine m_drainCredit;
    public int CurrentCredit => m_player.CurrentStats.Property.Health;
    void OnEnable()
    {
        m_player.OnHeal += RefreshDisplay;
        m_player.OnTakeDamage += RefreshDisplay;
        InternalEventHandler.ListenToGlobal(GameEventTriggerType.CreditOverTime, OnCreditDrain);
    }
    void OnDisable()
    {
        m_player.OnHeal -= RefreshDisplay;
        m_player.OnTakeDamage -= RefreshDisplay;
        InternalEventHandler.UnlistenFromGlobal(GameEventTriggerType.CreditOverTime, OnCreditDrain);
    }
    public void Init()
    {
        RefreshDisplay(CurrentCredit);
    }
    void RefreshDisplay(int change) 
    {
        char sign = change < 0 ? ' ' : '+';
        m_animate.color = change < 0 ? Color.red : Color.green;
        m_animate.text = $"{sign}{change}";
        m_onCreditUpdate?.Invoke();
        m_display.text = CurrentCredit.ToString();
    }
    public void RewardPoints(int add) 
    {
        m_player.Heal(Mathf.Abs(add));
    }
    public void OnObjectiveReward(DeliveryDetail obj)
    {
        RewardPoints(obj.PointReward);
        // Additonal rewards
        Debug.Log("Reward: Credit + " + obj.PointReward);
    }
    public void OnPayment(int pay) 
    {
        m_player.TakeDamage(pay);       
    }
    void OnCreditDrain(object sender, KDEventInfo args)
    {
        if (args.Payload == null) return;
        bool activate = false;
        float timeInterval = 0f;
        int changePerTick = 1;
        // 
        if (args.Payload.TryGetValue("isOn", out object t) && t is bool isOn)
        {
            activate = isOn;
        }
        if (args.Payload.TryGetValue("timeInterval", out object p) && p is float interval)
        {
            timeInterval = interval;
        }
        if (args.Payload.TryGetValue("delta", out object d) && d is int delta)
        {
            changePerTick = delta;
        }
        // Toggle off and restart drain effect
        StopCoroutine(m_drainCredit);
        if (activate && timeInterval <= 0f)
        {
            // create tick call depending on taking heal or damage
            Action tick = changePerTick < 0 ? () => { m_player?.TakeDamage(changePerTick); }
            :
                () => { m_player?.Heal(changePerTick); };
            m_drainCredit = StartCoroutine(CreditActionOverTime(timeInterval, tick));
        }
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
