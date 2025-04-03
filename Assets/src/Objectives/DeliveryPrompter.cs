using Curry.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct DeliveryDetail 
{
    public int DestinationIndex;
    public DeliveryType DeliverType;
    public int PointReward;
    public string Title;
    [TextArea(5, 10)]
    public string Description;
}
// handler UI for delivery objective
public delegate void OnTimeOut<T>(T sender);
public class DeliveryPrompter : MonoBehaviour 
{
    [SerializeField] List<DeliveryIcon> m_currentIcons = default;
    [SerializeField] DirectionPointer m_pointer = default;
    [SerializeField] LocationHandler m_locations = default;
    int m_numActive = 0;
    Predicate<DeliveryIcon> GetIcon(DeliveryObjective obj) => (i) => i.CurrentDelivery.Title == obj.Title;
    public void NewDelivery(IObjective newDelivery) 
    {
        if (newDelivery == null || m_numActive >= m_currentIcons.Count) return;
        if (newDelivery is DeliveryObjective obj) 
        {
            m_pointer.gameObject.SetActive(true);
            m_currentIcons[m_numActive].StartIcon(obj);
            var loc = m_locations.GetLocation(obj.Detail.DestinationIndex);
            m_pointer?.UpdatePointingTarget(loc);
            m_numActive++;
        }
    }
    public void OnDeliverySuccess(IObjective obj) 
    {
        if (obj is DeliveryObjective succ)
        {
            DeliveryIcon icon = m_currentIcons.Find(GetIcon(succ));
            icon?.ResetIcon();
            m_pointer.gameObject.SetActive(false);
        }
    }
    public void OnDeliveryFail(IObjective obj) 
    {
        if (obj is DeliveryObjective fail)
        {
            DeliveryIcon icon = m_currentIcons.Find(GetIcon(fail));
            icon?.ResetIcon();
            m_pointer.gameObject.SetActive(false);
        }
    }  
}
