using Curry.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct DeliveryDetail : IObjective
{
    public int DestinationIndex;
    public DeliveryType DeliverType;
    public int PointReward;
    public string Title;
    [TextArea(5, 10)]
    public string Description;
    string IObjective.Title => Title;
    string IObjective.Description => Description;
}
// handler UI for delivery objective
public class DeliveryPrompter : MonoBehaviour 
{
    [SerializeField] List<DeliveryIcon> m_currentIcons = default;
    [SerializeField] List<DirectionPointer> m_pointers = default;
    [SerializeField] LocationHandler m_originsFood = default;
    [SerializeField] LocationHandler m_originsIntel = default;
    [SerializeField] LocationHandler m_originsContraband = default;

    [SerializeField] LocationHandler m_destinations = default;

    int m_numActive = 0;
    Predicate<DeliveryIcon> GetIcon(DeliveryDetail obj) => (i) => i.CurrentDelivery.Title == obj.Title;
    public void NewDelivery(DeliveryDetail newDelivery) 
    {
        if (m_numActive >= m_currentIcons.Count) return;
        m_currentIcons[m_numActive].StartIcon(newDelivery);
        var loc = m_originsFood.GetLocation(newDelivery.DestinationIndex);
        NewObjective(loc);
        m_numActive++;    
    }
    public void OnDeliverySuccess(DeliveryDetail obj) 
    {
        DeliveryIcon icon = m_currentIcons.Find(GetIcon(obj));
        int index = m_currentIcons.IndexOf(icon);
        icon?.ResetIcon();
        StopPointing(index);
    }
    public void OnDeliveryFail(DeliveryDetail obj) 
    {
        DeliveryIcon icon = m_currentIcons.Find(GetIcon(obj));
        int index = m_currentIcons.IndexOf(icon);
        icon?.ResetIcon();
        StopPointing(index);
    }

    public int NewObjective(Transform newTarget)
    {
        for (int i = 0; i < m_pointers.Count; i++)
        {
            if (!m_pointers[i].IsPointing)
            {
                m_pointers[i].gameObject.SetActive(true);
                m_pointers[i].PointToward(newTarget);
                return i;
            }
        }
        return -1;
    }
    public void StopPointing(int index)
    {
        if (index < 0 || index >= m_pointers.Count) return;
        m_pointers[index]?.StopPointing();
        m_pointers[index].gameObject.SetActive(false);
    }
    public void SetObjective(int index, Transform newTarget)
    {
        if (index < 0 || index >= m_pointers.Count) return;
        m_pointers[index]?.PointToward(newTarget);
    }
}
