using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// listens to a collection of charge units,
// trigger a charge attack when all units in group is charged
// if fail and no more retries, trigger fail event
public delegate void OnChargeGroupUpdate(ChargeAttackGroup toUpdate);
public class ChargeAttackGroup : MonoBehaviour 
{
    [SerializeField] float m_chargeTimeInterval = default;
    [SerializeField] List<ChargeUnit> m_chargeUnits = default;
    // fire beam here
    [SerializeField] UnityEvent m_onChargeAttack = default;
    // when lanes fail enough times
    [SerializeField] UnityEvent m_onChargeUnitFail = default;
    public event OnChargeGroupUpdate OnChargeFinish = default;
    bool m_readyToStrike;
    bool m_isCharging = false;
    // no. lanes failed
    int m_numFail = 0;
    public List<ChargeUnit> ChargeUnits => m_chargeUnits;
    public bool ReadyToStrike { get => m_readyToStrike; private set => m_readyToStrike = value; }
    int m_numCharged = 0;
    void OnEnable()
    {
        ReadyToStrike = false;
        foreach (var item in ChargeUnits)
        {
            item.OnFinish += OnUnitFinish;
            item.OnCancel += OnChargeUnitFail;
        }
    }
    void OnDisable()
    {
        ReadyToStrike = false;
        foreach (var item in ChargeUnits)
        {
            item.OnFinish -= OnUnitFinish;
            item.OnCancel -= OnChargeUnitFail;
        }
    }
    public void ResetUnits() 
    {
        StopAllCoroutines();
        foreach (var item in ChargeUnits)
        {
            item?.ResetCharge();
        }
        ReadyToStrike = false;
        m_isCharging = false;
    }
    void OnChargeUnitFail(ChargeUnit item)
    {
        m_numFail++;
        // enter fail state
        if (m_numFail > ChargeUnits.Count)
        {
            m_numFail = 0;
            m_onChargeUnitFail?.Invoke();
        }
    }
    // call this to start charging sequence
    public void BeginCharging()
    {
        if (m_isCharging) return;
        m_isCharging = true;
        ResetUnits();
        StartCoroutine(BeginCharging_Internal());
    }
    public void Activate() 
    {
        if (!ReadyToStrike) return;
        // reset charge states
        m_numCharged = 0;
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
        }
    }
    void ChargeUnit(ChargeUnit toCharge)
    {
        // get cooldown
        toCharge?.BeginCharging();
    }
    IEnumerator BeginCharging_Internal()
    {
        foreach (var item in m_chargeUnits)
        {
            yield return new WaitForSeconds(m_chargeTimeInterval);
            ChargeUnit(item);
        }
    }
}