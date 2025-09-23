
using UnityEngine;
// Pause/unpause game timer
public class StopWatch : Item 
{
    [SerializeField] float m_pointDrainDeltaTime = default;
    bool m_stopped = false;
    public override void UseItem()
    {
        base.UseItem();
        // call to pause/resume time
        m_stopped = !m_stopped;
        KDEventUtil.PauseTimer(this, m_stopped);
    }
}
