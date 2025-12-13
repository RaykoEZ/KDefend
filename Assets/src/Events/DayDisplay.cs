using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DayDisplay : MonoBehaviour 
{
    TextMeshProUGUI Text => GetComponent<TextMeshProUGUI>();
    public void DisplayDay(DayOfWeek dayIndex) 
    {
        Text.text = DayCounter.s_dayOfWeekText_Short[(int)dayIndex];
    }
}