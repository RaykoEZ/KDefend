using System.Collections;
using Curry.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
// For NPC charged attacks with multiple stages/summons, if this completes charging,
// feedback to handler trigger charged attack
public delegate void OnChargeUnitUpdate(ChargeUnit toUpdate);
public class ChargeUnit : MonoBehaviour 
{
    [SerializeField] float m_chargingDuration = default;
    [SerializeField] PlayableAsset m_chargeLoop = default;
    [SerializeField] PlayableAsset m_chargeFinishing = default;
    [SerializeField] PlayableDirector m_sequencer = default;
    [SerializeField] UnityEvent OnChargeFinish = default;
    [SerializeField] UnityEvent OnChargeFail = default;
    public event OnChargeUnitUpdate OnFinish;
    public event OnChargeUnitUpdate OnCancel;
    bool m_charging = false;
    public void BeginCharging() 
    {
        if (m_charging) return;
        StartCoroutine(Charging());
    }
    public void CancelCharging() 
    {
        ResetCharge();
        OnCancel?.Invoke(this);
        OnChargeFail?.Invoke();
    }
    public void ResetCharge() 
    {
        m_charging = false;
        m_sequencer.Stop();
    }
    public void OnFinished() 
    {
        m_charging = false;
        OnChargeFinish?.Invoke();
        OnFinish?.Invoke(this);
    }
    IEnumerator Charging() 
    {
        m_charging = true;
        GameUtil.PlayActivationSequence(m_sequencer, m_chargeLoop, DirectorWrapMode.Loop);
        yield return new WaitForSeconds(m_chargingDuration);
        GameUtil.PlayActivationSequence(m_sequencer, m_chargeFinishing, DirectorWrapMode.Hold);
    }
}