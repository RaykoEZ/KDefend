using System;
using Curry.Explore;
using Curry.UI;
using TMPro;
using UnityEngine;
[Serializable]
public struct DisplayInfo 
{
    public string Name;
    public BaseEntity ObjectReference;
}
public class EnemyHUDHandler : HideableUI 
{
    [SerializeField] TextMeshProUGUI m_name = default;
    [SerializeField] ResourceDisplayHandler m_hpDisplay = default;
    BaseEntity m_displayingRef;

    public void DisplayBoss(DisplayInfo info) 
    {
        if (info.ObjectReference == null) return;
        m_displayingRef = info.ObjectReference;
        m_displayingRef.OnTakeDamage += UpdateHp;
        m_displayingRef.OnHeal += UpdateHp;
        m_hpDisplay.SetMaxValue(m_displayingRef.BaseStats.Property.Health);
        m_hpDisplay.SetCurrentValue(m_displayingRef.CurrentStats.Property.Health);
        if (m_name != null) 
        {
            m_name.text = info.Name;
        }
        Show();
    }
    public void UpdateHp(int _) 
    {
        m_hpDisplay.SetCurrentValue(m_displayingRef.CurrentStats.Property.Health);
    }
    public override void Hide()
    {
        m_displayingRef.OnTakeDamage -= UpdateHp;
        m_displayingRef.OnHeal -= UpdateHp;
        base.Hide();
    }
}