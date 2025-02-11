using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttackRotation : MonoBehaviour 
{
    [SerializeField] float m_attackRangeRadius = default;
    public virtual void TryAttack(List<BaseWeapon> weapons, BaseEntity target)
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance <= m_attackRangeRadius)
        {
        }
    }
}
