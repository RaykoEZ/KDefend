using UnityEngine.Events;
using UnityEngine;
using TMPro;
// Handles player reward and payment
public class PlayerCreditManager : MonoBehaviour
{
    [SerializeField] Player m_player = default;
    [SerializeField] TextMeshProUGUI m_display = default;
    [SerializeField] TextMeshProUGUI m_animate = default;
    [SerializeField] UnityEvent m_onCreditUpdate = default;
    public int CurrentCredit => m_player.CurrentStats.Property.Health;
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
}
