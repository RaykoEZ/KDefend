using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// attacks in a 360 rotation, direction is evenly split between each weapon shot
public class WaveStrike : ActiveAbility
{
    // Angle of range to shoot
    [Range(0, 360f)]
    [SerializeField] protected float m_attackArc = default; 
    [Range(3, 18)]
    // number of attack segments per wave, calcs for angle interval
    [SerializeField] int m_numAttacksPerWave = default;
    [SerializeField] AttackHandler m_attackHandler = default;
    [SerializeField] List<BaseWeapon> m_weaponRotation = default;
    protected float m_angleInterval = 0f;
    public float StrikeTimeInterval { get => m_cooldownTime; set => m_cooldownTime = value; }

    void OnEnable()
    {
        m_angleInterval = m_attackArc / m_numAttacksPerWave;
    }
    protected override void Effect_Internal()
    {
        Attack();
    }
    protected void Attack() 
    {
        if (m_weaponRotation.Count == 0) return;
        Vector2 dir = Vector2.up;
        Vector3 rotatedDir;
        float angle = VectorExtension.DegreeFromDirection(dir);
        int weaponIndex = 0;
        // for each strike angle interval, attack with weapon from rotation list
        for (int i = 0; i < m_numAttacksPerWave; i++) 
        {
            rotatedDir = VectorExtension.VectorFromDegree(angle);
            m_attackHandler.UseWeaponOneShot(m_weaponRotation[weaponIndex], rotatedDir);
            //increment or reset weapon rotation index
            weaponIndex = weaponIndex + 1 >= m_weaponRotation.Count? 0 : weaponIndex++;
            angle -= m_angleInterval;
        }
    }
}
