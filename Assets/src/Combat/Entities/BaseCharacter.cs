using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Anything that can be pushed away by a force
public interface IPushable
{
    public void Push(Vector2 dir, float power);
}
public class BaseCharacter : BaseEntity , IPushable
{
    [SerializeField] protected List<BaseWeapon> m_weapons = default;
    protected List<bool> m_attackingWeapons;
    protected bool m_keepFiring = false;
    protected override void Awake()
    {
        base.Awake();
        m_attackingWeapons = new List<bool>(m_weapons.Count);
        for (int i = 0; i < m_weapons.Count; i++)
        {
            m_attackingWeapons.Add(false);
        }
    }
    protected virtual Vector2 GetAimDirection()
    {
        return Vector2.up;
    }
    public virtual void UseWeapon() 
    {
        if (m_keepFiring) return;
        for (int i = 0; i < m_weapons.Count; i++)
        {
            if (m_attackingWeapons[i]) continue;
            StartCoroutine(AttackCycle(i));
        }
    }
    IEnumerator AttackCycle(int weaponIndex) 
    {
        BaseWeapon weapon = m_weapons[weaponIndex];
        if (weapon == null) yield break;
        m_attackingWeapons[weaponIndex] = true;
        while (m_attackingWeapons[weaponIndex])
        {
            //fire cycle
            yield return weapon.Attack(weapon, transform, GetAimDirection(), weapon.InstantiateWeapon);
            // next firing cycle
            yield return new WaitForSeconds(weapon.Property.DelayPerCycle);
            // hold fire >> continue cycle
            m_attackingWeapons[weaponIndex] = m_keepFiring;
        }
    }
    public void Push(Vector2 dir, float power)
    {
        StartCoroutine(Push_Internal(dir, power));
    }
    protected virtual IEnumerator Push_Internal(Vector2 dirNormalize, float power)
    {
        GetComponent<Rigidbody2D>()?.AddForce(dirNormalize * power, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
    }
}
