
using UnityEngine;
// Pause/unpause game timer - tool0x
public class StopWatch : Item 
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

// Rewind game day 13x to Monday, Full Access: Remove self from game
public class HelloWorld : Item
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
