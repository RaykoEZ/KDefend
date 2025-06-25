using System.Collections.Generic;
using UnityEngine;
using Curry.Events;

public class IntelDrop : MonoBehaviour
{
    [SerializeField] int m_threatValueIncrease = default;
    [SerializeField] List<SpawnWave> m_possibleWaves = default;
    [SerializeField] CurryGameEventTrigger m_pickupDelivery = default;
    // Trigger a boss wave with a decrypting device on boss
    public void PickupIntel()
    {
        if (m_possibleWaves.Count == 0) return;
        // pick a random wave to spawn
        int rand = Random.Range(0, m_possibleWaves.Count);
        SpawnWave wave = m_possibleWaves[rand];
        Dictionary<string, object> payload = new Dictionary<string, object> 
        { {"threatGain", m_threatValueIncrease },
            {"wave",  wave} };
        EventInfo info = new EventInfo(payload);
        m_pickupDelivery?.TriggerEvent(info);
    }
}