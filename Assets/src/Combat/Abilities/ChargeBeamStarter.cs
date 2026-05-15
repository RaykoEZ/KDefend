using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// spawn all charge units gradually
// when a group of units finish charging, fire beam in that direction
// if enough units fail, trigger fail sequence
public class ChargeBeamStarter : MonoBehaviour 
{
    [SerializeField] int m_numFailedUnitAllowed = default;
    [SerializeField] float m_spawnTimeInterval = default;
    [SerializeField] float m_cooldown = default;
    [SerializeField] List<ChargeUnit> m_chargeUnits = default;
    [SerializeField] List<ChargeAttackGroup> m_lanes = default;
    // when lanes fail enough times
    [SerializeField] UnityEvent m_onChargeUnitFail = default;
    bool m_isCharging = false;
    // no. lanes failed
    int m_numFail = 0;
    List<ChargeUnit> m_charging = new List<ChargeUnit>();
    List<ChargeUnit> m_idle = new List<ChargeUnit>();
    void Start()
    {
        m_idle = new List<ChargeUnit> (m_chargeUnits);
    }
    void OnEnable()
    {
        foreach (var lane in m_lanes) 
        {
            lane.OnLaneAttack += OnLaneFinish;
        }
        foreach (var unit in m_chargeUnits) 
        {
            unit.OnCancel += OnChargeUnitFail;
        }
    }
    void OnDisable()
    {
        foreach (var lane in m_lanes)
        {
            lane.OnLaneAttack -= OnLaneFinish;
        }
        foreach (var unit in m_chargeUnits)
        {
            unit.OnCancel -= OnChargeUnitFail;
        }
    }
    // call this to start charging sequence
    public void BeginCharging()
    {
        if (m_isCharging) return;
        m_isCharging = true;
        StartCoroutine(BeginCharging_Internal());
    }
    void ChargeUnit() 
    {
        if (m_idle.Count == 0) return;
        // Choose a random direction to start charging
        // If charge completes a lane, fire a beam in that lane
        int rand = UnityEngine.Random.Range(0, m_idle.Count);
        var item = m_idle[rand];
        // get cooldown
        item?.BeginCharging();
        m_charging?.Add(item);
        m_idle.Remove(item);
    }
    // cancel all charging
    public void StopAll() 
    {
        StopAllCoroutines();
        m_charging.Clear();
        m_idle.AddRange(m_chargeUnits);
    }
    public void OnChargeUnitFail(ChargeUnit item) 
    {
        m_numFail++;
        // enter fail state
        if (m_numFail > m_numFailedUnitAllowed)
        {
            m_numFail = 0;
            m_onChargeUnitFail?.Invoke();
        }
    }
    public void OnLaneFinish(ChargeAttackGroup item)
    {
        m_isCharging = false;
        // stop all other charge units
        StopAll();
        // charge again later
        StartCoroutine(GameUtil.Cooldown(m_cooldown, BeginCharging));
    }
    IEnumerator BeginCharging_Internal()
    {
        while (m_isCharging)
        {
            yield return new WaitForSeconds(m_spawnTimeInterval);
            ChargeUnit();
        }
    }
}

