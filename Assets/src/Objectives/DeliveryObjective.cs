using System;
using UnityEngine;
using Curry.Events;
public enum DeliveryType 
{ 
    Food,
    Intel,
    Smuggle
}
[Serializable]
public class DeliveryObjective : GameObjective
{
    [SerializeField] DeliveryDetail m_detail = default;
    public DeliveryDetail Detail { get => m_detail; }
    public virtual void Setup(DeliveryDetail detail) 
    {
        m_detail = detail;
        m_title = detail.Title;
        m_description = detail.Description;
    }
}
