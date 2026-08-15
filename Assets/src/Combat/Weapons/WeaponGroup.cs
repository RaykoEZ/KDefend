using System.Collections.Generic;
using System.Collections;
using UnityEngine;
// Use this to trigger multiple weapons at the same time
public class WeaponGroup : BaseWeapon
{
    [SerializeField] List<BaseWeapon> m_toFire = default;
    [SerializeField] bool m_focusFireOnStaticTarget = default;
    private Transform m_staticTarget;
    public override bool InstantiateWeapon { get; }
    public Transform FocusTarget { get => m_staticTarget; set => m_staticTarget = value; }
    public void LaunchAttack() 
    {
        if (m_focusFireOnStaticTarget && FocusTarget != null) 
        {
            Vector2 dir;
            foreach (var item in m_toFire)
            {
                dir = (FocusTarget.position - item.transform.position).normalized;
                item?.LaunchAttack(dir);
            }
        }
    }
    public override void LaunchAttack(Vector2 directionNormalized)
    {
        foreach (var item in m_toFire)
        {
            item?.LaunchAttack(directionNormalized);
        }       
    }
}
