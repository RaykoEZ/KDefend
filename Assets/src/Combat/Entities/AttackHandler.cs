using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Handle use of weapon for attacking
public abstract class AttackHandler : MonoBehaviour 
{
    [SerializeField] List<BaseWeapon> m_defaultWeapons = default;
    [SerializeField] protected UnityEvent m_onAttack = default;
    protected List<bool> m_attackingWeapons;
    protected bool m_keepFiring = false;
    protected List<BaseWeapon> m_currentWeapons = new List<BaseWeapon>();
    public bool KeepFiring { get => m_keepFiring; set => m_keepFiring = value; }
    public abstract Vector2 GetAimDirectionNormalized();
    protected virtual void OnEnable()
    {
        ResetWeapons();
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
    public virtual void UseWeapon()
    {
        if (m_keepFiring || m_currentWeapons.Count == 0) return;
        for (int i = 0; i < m_currentWeapons.Count; i++)
        {
            if (m_attackingWeapons[i]) continue;
            StartCoroutine(AttackCycle_Internal(i));
        }
    }
    // attack with a given weapon once
    public void UseWeaponOneShot(BaseWeapon toUse, Vector2 direction) 
    {
        StartCoroutine(Attack_Internal(toUse, direction));
    }
    protected virtual IEnumerator AttackCycle_Internal(int weaponIndex)
    {
        BaseWeapon weapon = m_currentWeapons[weaponIndex];
        if (weapon == null) yield break;
        m_attackingWeapons[weaponIndex] = true;
        while (m_attackingWeapons[weaponIndex])
        {
            //fire cycle
            yield return Attack_Internal(weapon, GetAimDirectionNormalized());
            // hold fire >> continue cycle
            m_attackingWeapons[weaponIndex] = m_keepFiring;
        }
    }
    protected IEnumerator Attack_Internal(BaseWeapon weapon, Vector2 direction)
    {
        //fire cycle
        m_onAttack?.Invoke();
        // get attack direction
        yield return weapon?.Attack(weapon, transform, direction, weapon.InstantiateWeapon);
        // next firing cycle
        yield return new WaitForSeconds(weapon.WeaponProperty.DelayPerCycle);

    }
    public void AddWeapon(BaseWeapon toAdd)
    {
        m_currentWeapons?.Add(toAdd);
        m_attackingWeapons.Add(false);
    }
    public void ResetWeapons()
    {
        SetWeapons(m_defaultWeapons);
    }
}
