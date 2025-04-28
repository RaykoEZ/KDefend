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
public delegate void OnTimeOut<T>(T sender);
public class DeliveryPrompter : MonoBehaviour 
{
    [SerializeField] List<
        DeliveryIcon> m_currentIcons = default;
    [SerializeField] DirectionPointer m_pointer = default;
    [SerializeField] LocationHandler m_locations = default;
    int m_numActive = 0;
    Predicate<DeliveryIcon> GetIcon(DeliveryDetail obj) => (i) => i.CurrentDelivery.Title == obj.Title;
    public void NewDelivery(DeliveryDetail newDelivery) 
    {
        if (m_numActive >= m_currentIcons.Count) return;
        m_pointer.gameObject.SetActive(true);
        m_currentIcons[m_numActive].StartIcon(newDelivery);
        var loc = m_locations.GetLocation(newDelivery.DestinationIndex);
        m_pointer?.UpdatePointingTarget(loc);
        m_numActive++;
        
    }
    public void OnDeliverySuccess(DeliveryDetail obj) 
    {
        DeliveryIcon icon = m_currentIcons.Find(GetIcon(obj));
        icon?.ResetIcon();
        m_pointer.gameObject.SetActive(false);
    }
    public void OnDeliveryFail(DeliveryDetail obj) 
    {
        DeliveryIcon icon = m_currentIcons.Find(GetIcon(obj));
        icon?.ResetIcon();
        m_pointer.gameObject.SetActive(false);
    }  
}
