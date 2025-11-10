using UnityEngine;
/// <summary>
/// Each Lv: + 100 seconds to day length, special boss upon reaching maxed out time limit
/// </summary>
public class Overtime : Collectible 
{
    [SerializeField] int m_extendTime = default;
    // on obtaining, extend max time
    public override void UseItem()
    {
        base.UseItem();
        KDEventUtil.ExtendTimer(this, m_extendTime);
    }
}
