using Curry.Events;
using Curry.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Handles player reward and payment
public class PlayerCreditManager : MonoBehaviour
{
    [Range(1, 9999999)]
    [SerializeField] int m_targetCredit = default;
    [SerializeField] Player m_player = default;
    [SerializeField] TextMeshProUGUI m_display = default;
    [SerializeField] TextMeshProUGUI m_animate = default;
    [SerializeField] ResourceDisplayHandler m_hpBar = default;
    [SerializeField] UnityEvent m_onCreditUpdate = default;
    [SerializeField] UnityEvent<int> m_onCreditLoss = default;
    [SerializeField] UnityEvent<int> m_onCreditGain = default;
    [SerializeField] UnityEvent m_onCreditTargetReached = default;
    Dictionary<string, CreditChangeOverTime> m_changesOverTime = new Dictionary<string, CreditChangeOverTime>();
    public int CurrentCredit => m_player.CurrentStats.Property.Health;
    public int TargetCredit { get => m_targetCredit; set => m_targetCredit = value; }

    void OnEnable()
    {
        m_player.OnHeal += RefreshDisplay;
        m_player.OnTakeDamage += RefreshDisplay;
        m_player.SetBaseHp(TargetCredit);
        m_hpBar.SetMaxValue(TargetCredit);
        m_hpBar.SetCurrentValue(m_player.CurrentStats.Property.Health);
        KDEventHandler.ListenToGlobal(GameEventTriggerType.CreditOverTime, CreditOverTime);
    }
    void OnDisable()
    {
        m_player.OnHeal -= RefreshDisplay;
        m_player.OnTakeDamage -= RefreshDisplay;
        KDEventHandler.UnlistenFromGlobal(GameEventTriggerType.CreditOverTime, CreditOverTime);
    }
    public void Init()
    {
        RefreshDisplay(CurrentCredit);
    }
    void RefreshDisplay(int change) 
    {
        if (change == 0) return;       
        bool isLosingCredit = change < 0;
        char sign = isLosingCredit ? ' ' : '+';
        m_animate.color = isLosingCredit ? Color.red : Color.green;
        m_animate.text = $"{sign}{change}";
        m_onCreditUpdate?.Invoke();
        if (isLosingCredit) 
        {
            m_onCreditLoss?.Invoke(change);
        }
        else 
        {
            m_onCreditGain?.Invoke(change);
        }
        m_hpBar.SetMaxValue(TargetCredit);
        m_hpBar.SetCurrentValue(m_player.CurrentStats.Property.Health);
        m_display.text = $"{CurrentCredit.ToString()} / {m_targetCredit}";
        // when credit target reached, call level event
        if (CurrentCredit >= m_targetCredit) 
        {
            m_onCreditTargetReached?.Invoke();
        }
    }
    public void RewardPoints(int add) 
    {
        m_player.Heal(Mathf.Abs(add));
    }
    public void OnObjectiveReward(DeliveryDetail obj)
    {
        RewardPoints(obj.PointReward);
        // Additonal rewards
    }
    public void OnPayment(int pay) 
    {
        m_player.TakeDamage(pay);       
    }
    // handle credit change over time
    void CreditOverTime(object sender, KDEventInfo args)
    {
        if (args.Payload == null) return;
        bool activate = false;
        float timeInterval = 0f;
        int changePerTick = 1;
        // get params from event args
        string name = sender is MonoBehaviour mono ? mono.gameObject.name : gameObject.name;
        if (args.Payload.TryGetValue("isOn", out object i) && i is bool isOn)
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
        // find existing credit changes in collection
        CreditChangeOverTime toChange;        
        if (m_changesOverTime.TryGetValue(name, out CreditChangeOverTime changer)) 
        {
            toChange = changer;
        }
        else 
        {
            toChange = new CreditChangeOverTime(m_player, changePerTick, timeInterval);
            m_changesOverTime.Add(name, toChange);
        }
        if (activate)
        {
            toChange?.Begin();
        }else 
        { 
            toChange?.End();
        }
    }
}
