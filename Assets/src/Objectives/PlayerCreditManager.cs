using Curry.Events;
using UnityEngine;
// Handles player reward and payment
public class PlayerCreditManager : MonoBehaviour
{
    [SerializeField] Player m_player = default;
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
        if (m_player.CurrentStats.Health <= pay) 
        {
            // cannot afford, Notify
        }
        else 
        {
            m_player.TakeDamage(pay);
        }
    }
}
