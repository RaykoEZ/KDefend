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
    [SerializeField] int m_delayPerAttack = default;
    [SerializeField] int m_baseDamage = default;
    int m_currentDamage = 1;
    public int CurrentDamage { get => m_currentDamage; set => m_currentDamage = value; }

    void Start()
    {
        m_currentDamage = m_baseDamage;
    }
}

// Create clue to hidden boss
public class RILetter : Collectible
{
    [SerializeField] string m_folderName = default;
    [SerializeField] FileWriteDetail m_fileWriteDetail = default;
    [SerializeField] Texture2D m_photoSent = default;
    public override void UseItem()
    {
        base.UseItem();
        FileWriter writer = new FileWriter();
        // write to player desktop, do it silently if possible
        writer.WriteToDesktop(m_fileWriteDetail.Filename, m_folderName, m_fileWriteDetail.RawContent);
        writer.SendPngToDesktop(filename: "fromRI", foldername: m_folderName, m_photoSent);
    }
}
public class TrainingWeights : Collectible
{
    [SerializeField] float m_cooldownDuration = default;
}
public class Investment101 : Collectible 
{
    [SerializeField] GainCredit m_gainCredit = default;
    [SerializeField] int m_baseTimeInterval = default;
    int m_currentTimeInterval = 200;
    // let rank up handle time scaling
    public int CurrentTimeInterval { get => m_currentTimeInterval; set => m_currentTimeInterval = value; }
    void Start()
    {
        m_currentTimeInterval = m_baseTimeInterval;
    }
}
/// <summary>
/// Each Lv: + 100 seconds to day length, special boss upon reaching maxed out time limit
/// </summary>
public class Overtime : Collectible 
{
    [SerializeField] int m_extendTime = default;
    // on obtaining, extend max time
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
// Rewind game day 13x to Monday, Full Access: Remove self from game
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
