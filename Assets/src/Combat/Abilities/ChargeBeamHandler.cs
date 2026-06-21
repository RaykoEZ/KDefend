using UnityEngine;
using UnityEngine.Playables;
// spawn all charge units gradually
// when a group of units finish charging, fire beam in that direction
// if enough units fail, trigger fail sequence
public class ChargeBeamHandler : MonoBehaviour 
{
    [SerializeField] float m_cooldown = default;
    [SerializeField] ChargeAttackGroup m_chargeGroup = default;
    [SerializeField] PlayableDirector m_sequencer = default;
    void OnEnable()
    {
        m_chargeGroup.OnChargeFinish += OnGroupChargeFinish;
    }
    void OnDisable()
    {
        m_chargeGroup.OnChargeFinish -= OnGroupChargeFinish;
    }
    public void OnGroupChargeFinish(ChargeAttackGroup item)
    {
        // start sequence to activate attack pattern
        GameUtil.PlaySequence(m_sequencer);
    }
    // called after unleashing a charge attack
    public void OnAttackFinish() 
    {
        StartCoroutine(GameUtil.Cooldown(m_cooldown, m_chargeGroup.BeginCharging));
    }
}