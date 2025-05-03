using Curry.Events;
using UnityEngine;
using TMPro;
// Handles player reward and payment
public class PlayerCreditManager : MonoBehaviour
{
    [SerializeField] Player m_player = default;
    [SerializeField] TextMeshProUGUI m_display = default;
    void OnEnable()
    {
        m_player.OnHeal += RefreshDisplay;
        m_player.OnTakeDamage += RefreshDisplay;
    }
    void OnDisable()
    {
        m_player.OnHeal -= RefreshDisplay;
        m_player.OnTakeDamage -= RefreshDisplay;
    }
    public void Init()
    {
        RefreshDisplay(m_player.CurrentStats.Property.Health);
    }
    void RefreshDisplay(int newVal) 
    {
        m_display.text = newVal.ToString();
    }
    public void RewardPoints(int add) 
    {
        m_player.Heal(Mathf.Abs(add));
    }
    public void OnObjectiveReward(IObjective obj)
    {
        if (obj is DeliveryObjective succ)
        {
            RewardPoints(succ.Detail.PointReward);
            // Additonal rewards
        }
    }
    public void OnPayment(int pay) 
    {
        if (m_player.CurrentStats.Property.Health <= pay) 
        {
            // cannot afford, Notify
        }
        else 
        {
            m_player.TakeDamage(pay);
        }
    }
}
