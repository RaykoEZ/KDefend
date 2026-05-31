using System;
using System.Collections;
using Curry.Util;
using UnityEngine;

// attacks in a 360 rotation, direction is evenly split between each weapon shot
public class WaveStrike : UseWeapon
{
    // Angle of range to shoot
    [Range(0, 360f)]
    [SerializeField] protected float m_attackArc = default;
    protected float m_angleInterval = 0f;
    protected float m_currentAimAngle = 0f;
    void OnEnable()
    {
        m_angleInterval = m_attackArc / m_numAttacksPerCycle;
    }
    protected override Vector2 AimDirectionNormalized()
    {
        return VectorExtension.VectorFromDegree(m_currentAimAngle);
    }
    protected override void PrepareAttack()
    {
        // play initate sequence
        // sequence signal will trigger the attack frame during playback
        GameUtil.PlayActivationSequence(m_activationSequence);
    }
    protected override void PostAttack()
    {
        // update shooting angle
        m_currentAimAngle -= m_angleInterval;
    }
}