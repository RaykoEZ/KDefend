using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;

// listens to a collection of charge units,
// trigger a charge attack when all units in group is charged
// if fail and no more retries, trigger fail event
public delegate void OnChargeGroupUpdate(ChargeAttackGroup toUpdate);
public class ChargeAttackGroup : MonoBehaviour 
{
    [SerializeField] List<ChargeUnit> m_chargeUnits = default;
    // fire beam here
    [SerializeField] UnityEvent m_onChargeAttack = default;
    public event OnChargeGroupUpdate OnLaneAttack = default;
    public List<ChargeUnit> ChargeUnits => m_chargeUnits;
    HashSet<ChargeUnit> m_charged = new HashSet<ChargeUnit>();
    void OnEnable()
    {
        foreach (var item in ChargeUnits)
        {
            item.OnFinish += OnUnitFinish;
        }
    }
    void OnDisable()
    {
        foreach (var item in ChargeUnits)
        {
            item.OnFinish -= OnUnitFinish;
        }
    }
    void OnUnitFinish(ChargeUnit unit) 
    {
        m_charged.Add(unit);
        // check is all have chanrged
        if (m_charged.Count == m_chargeUnits.Count) 
        {
            m_charged.Clear();
            OnLaneAttack?.Invoke(this);
            m_onChargeAttack?.Invoke();
        }
    }
}

