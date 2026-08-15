using System.Collections;
using Curry.UI;
using PixelCrushers.DialogueSystem;
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
    [SerializeField] PlayableAsset m_onDefeat = default;
    [SerializeField] PlayableDirector m_sequencer = default;
    [SerializeField] UnityEvent OnChargeFinish = default;
    [SerializeField] UnityEvent OnChargeInterrupt = default;
    public event OnChargeUnitUpdate OnFinish;
    public event OnChargeUnitUpdate OnCancel;
    bool m_operational = true;
    Coroutine m_charge;
    // for temporarily disabling this unit in scene events
    public bool Operational { get => m_operational; set => m_operational = value; }

    public void BeginCharging() 
    {
        if (m_charge != null || !Operational) return;
        m_charge = StartCoroutine(Charging());
    }
    public void CancelCharging() 
    {
        ResetCharge();
        OnCancel?.Invoke(this);
    }
    public void ResetCharge() 
    {
        if (m_charge != null)
        {
            StopCoroutine(m_charge);
            m_charge = null;
        }
    }
    public void Shutdown() 
    {
        m_sequencer?.Play(m_onDefeat);
    }
    public void OnFinished() 
    {
        OnChargeFinish?.Invoke();
        OnFinish?.Invoke(this);
        ResetCharge();
    }
    IEnumerator Charging() 
    {
        yield return new WaitForEndOfFrame();
        // check if the core was disabled on the same frame it tried to charge up
        if (!Operational) 
        {
            OnChargeInterrupt?.Invoke();
            yield break;
        }
        GameUtil.PlaySequence(m_sequencer, m_chargeLoop, DirectorWrapMode.Loop);
        yield return new WaitForSeconds(m_chargingDuration);
        GameUtil.PlaySequence(m_sequencer, m_chargeFinishing, DirectorWrapMode.Hold);
    }
}