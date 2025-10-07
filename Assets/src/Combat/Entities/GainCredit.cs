using System.Security.Cryptography;
using UnityEngine;
public class GainCredit : MonoBehaviour 
{
    [Range(1, 1000000)]
    [SerializeField] int m_minGain = default;
    [Range(2, 1000000)]
    [SerializeField] int m_maxGain = default;
    public void AddToCredit(Player player) 
    {
        int gain = m_maxGain <= m_minGain ? m_minGain : Random.Range(m_minGain, m_maxGain);
        player?.Heal(gain);
    }
}
