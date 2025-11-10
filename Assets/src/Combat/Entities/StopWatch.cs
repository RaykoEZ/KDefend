using UnityEngine;
// Pause/unpause game timer - tool0x
public class StopWatch : Collectible
{
    // time interval for each drain tick
    [SerializeField] float m_creditDrainInterval = default;
    [SerializeField] int m_creditDrainPerTick = default;
    bool m_isOn = false;
    bool m_fullAccess = false;
    public bool IsOn => m_isOn;
    public bool FullAccess { get => m_fullAccess; set => m_fullAccess = value; }
    // toggle time stop
    public override void UseItem()
    {
        base.UseItem();
        // call to pause/resume time
        m_isOn = !m_isOn;
        KDEventUtil.Pause_0x(this, m_isOn, m_fullAccess);
        KDEventUtil.CreditChangeOverTime(this, m_isOn, m_creditDrainInterval, m_creditDrainPerTick);
    }
}

// Assist from sniper, periodic lock-on attack on a random nearby enemy (use radius checks around player)
public class SniperInsignia : Collectible
{
    [SerializeField] int m_attackInterval = default;
    [SerializeField] int m_baseDamage = default;
    int m_currentDamage = 1;
    public int CurrentDamage { get => m_currentDamage; set => m_currentDamage = value; }

    void Start()
    {
        m_currentDamage = m_baseDamage;
    }
}
/// <summary>
/// Lv:
/// 1. Attract Enemies in range, 10% chance to stun in range.
/// 2. +10% chance to stun
/// 3. 10% chance to charm enemies (attack enemies too)
/// </summary>
public class NeverIdol : Collectible 
{
    [SerializeField] int m_attackInterval = default;


    // on obtain: creat field of attraction, damage over time & chance to stun
    // on rank up, increase chance to stun, damage, field size
    // on max rank, replace stun to berserk (NPC attack indiscriminately)
    public override void UseItem()
    {
        base.UseItem();

    }
}
// Screen-wide wipe effect, gain all credit from killed targets, progress to next day
// Rewind game day to Monday,
// Full Access: Target self to Remove self from game
public class HelloWorld : Collectible
{
    bool m_fullAccess = false;
    public bool FullAccess { get => m_fullAccess; set => m_fullAccess = value; }
    public override void UseItem()
    {
        base.UseItem();
        // call to pause/resume time
        KDEventUtil.HelloWorld_13x(this, m_fullAccess);
    }
}
