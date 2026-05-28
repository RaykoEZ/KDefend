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
    public event OnChargeGroupUpdate OnChargeFinish = default;
    bool m_readyToStrike;
    public List<ChargeUnit> ChargeUnits => m_chargeUnits;
    public bool ReadyToStrike { get => m_readyToStrike; private set => m_readyToStrike = value; }
    int m_numCharged = 0;
    void OnEnable()
    {
        ReadyToStrike = false;
        foreach (var item in ChargeUnits)
        {
            item.OnFinish += OnUnitFinish;
        }
    }
    void OnDisable()
    {
        ReadyToStrike = false;
        foreach (var item in ChargeUnits)
        {
            item.OnFinish -= OnUnitFinish;
        }
    }
    void ResetUnits() 
    {
        foreach (var item in ChargeUnits)
        {
            item?.ResetCharge();
        }
        ReadyToStrike = false;
    }
    public void Activate() 
    {
        if (!ReadyToStrike) return;
        m_onChargeAttack?.Invoke();
    }
    void OnUnitFinish(ChargeUnit unit) 
    {
        m_numCharged++;
        // check is all have chanrged
        if (m_numCharged == m_chargeUnits.Count) 
        {
            // set this to ready
            ReadyToStrike = true;
            // attack
            OnChargeFinish?.Invoke(this);
            // reset charge states
            m_numCharged = 0;
            ResetUnits();
        }
    }
}

