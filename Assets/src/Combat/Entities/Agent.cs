using UnityEngine;
using System.Collections.Generic;
//enemy agent behaviour
[RequireComponent(typeof(Enemy))]
public class Agent : MonoBehaviour
{
    [SerializeField] BackstepStrike m_backStep = default;
    [SerializeField] Stealth m_stealth = default;
    [SerializeField] Reinforcement m_callHelp = default;
    Enemy Self => GetComponent<Enemy>();
    // hits taken
    int m_hitsTaken = 0;
    public void OnTakeHit() 
    {
        m_hitsTaken++;
        float rand = Random.Range(0f, 1f);
        if (m_hitsTaken > 2 && rand < m_hitsTaken * 0.2f) 
        {
            BackstepStrike();
        }
        
        if (Self.HpRatio < 0.5) 
        {
            Debug.Log("Agent Low HP ratio: "+ Self.HpRatio);
            Stealth();
            Reinforce();
        }
    }
    // when taking damage, try back step 
    void BackstepStrike() 
    {
        if (m_backStep.TryUse()) 
        {
            m_hitsTaken = 0;
        }
    }
    // when < 50% HP, activate stealth & calls help
    void Stealth() 
    {
        m_stealth?.TryUse();
    }
    // call help
    void Reinforce() 
    {
        m_callHelp?.TryUse();
    }
}

public class Reinforcement : ActiveAbility
{
    [SerializeField] List<SpawnWave> m_reinforce = default;
    protected override void Effect_Internal()
    {
        throw new System.NotImplementedException();
    }
}
// dashes back and attacks with an attack
public class BackstepStrike : ActiveAbility 
{
    [SerializeField] float m_distance = default;

    protected override void Effect_Internal()
    {
        throw new System.NotImplementedException();
    }
}
// Hides self for some time
public class Stealth : ActiveAbility
{
    [SerializeField] float m_duration = default;

    protected override void Effect_Internal()
    {
        throw new System.NotImplementedException();
    }
}