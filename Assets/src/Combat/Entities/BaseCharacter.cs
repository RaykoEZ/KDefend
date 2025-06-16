using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Anything that can be pushed away by a force
public interface IPushable
{
    public void Push(Vector2 dir, float power);
}
public class BaseCharacter : BaseEntity , IPushable
{
    [SerializeField] List<BaseWeapon> m_defaultWeapons = default;
    [SerializeField] protected UnityEvent m_onAttack = default;
    protected List<bool> m_attackingWeapons;
    protected bool m_keepFiring = false;
    protected List<BaseWeapon> m_currentWeapons = new List<BaseWeapon>();
    public List<BaseWeapon> GetWeapons()
    {
        return m_currentWeapons;
    }
    public void SetWeapons(List<BaseWeapon> value)
    {
        if (value == null) return;
        m_currentWeapons = value;
        m_attackingWeapons = new List<bool>(m_currentWeapons.Count);
        for (int i = 0; i < m_currentWeapons.Count; i++)
        {
            m_attackingWeapons.Add(false);
        }
    }
    public void ResetWeapons()
    {
        SetWeapons(m_defaultWeapons);
    }
    protected override void Awake()
    {
        base.Awake();
        ResetWeapons();
    }
    protected virtual Vector2 GetAimDirection()
    {
        return Vector2.up;
    }
    public virtual void UseWeapon() 
    {
        if (m_keepFiring) return;
        for (int i = 0; i < m_currentWeapons.Count; i++)
        {
            if (m_attackingWeapons[i]) continue;
            StartCoroutine(AttackCycle_Internal(i));
        }
    }

    protected virtual IEnumerator AttackCycle_Internal(int weaponIndex) 
    {
        BaseWeapon weapon = m_currentWeapons[weaponIndex];
        if (weapon == null) yield break;
        m_attackingWeapons[weaponIndex] = true;
        while (m_attackingWeapons[weaponIndex])
        {
            //fire cycle
            yield return Attack_Internal(weapon);
            // hold fire >> continue cycle
            m_attackingWeapons[weaponIndex] = m_keepFiring;
        }
    }
    protected IEnumerator Attack_Internal(BaseWeapon weapon)
    {
        //fire cycle
        m_onAttack?.Invoke();
        yield return weapon?.Attack(weapon, transform, GetAimDirection(), weapon.InstantiateWeapon);
        // next firing cycle
        yield return new WaitForSeconds(weapon.WeaponProperty.DelayPerCycle);

    }
    public void Push(Vector2 dir, float power)
    {
        float rand = UnityEngine.Random.Range(0.5f, 5f);
        StartCoroutine(Push_Internal(dir, power * rand));
    }
    protected virtual IEnumerator Push_Internal(Vector2 dirNormalize, float power)
    {
        GetComponent<Rigidbody2D>()?.AddForce(dirNormalize * power, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
    }
}
