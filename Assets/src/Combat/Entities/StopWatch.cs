
using UnityEngine;
// Pause/unpause game timer - tool0x
public class StopWatch : Item 
{
    [SerializeField] float m_pointDrainDeltaTime = default;
    bool m_stopped = false;
    bool m_fullAccess = false;

    public bool FullAccess { get => m_fullAccess; set => m_fullAccess = value; }

    public override void UseItem()
    {
        base.UseItem();
        // call to pause/resume time
        m_stopped = !m_stopped;
        KDEventUtil.Pause_0x(this, m_stopped, m_fullAccess);
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
