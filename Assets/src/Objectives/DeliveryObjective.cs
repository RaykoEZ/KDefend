using System;
using UnityEngine;
using Curry.Events;
public enum DeliveryType 
{ 
    Food,
    Smuggle
}
[Serializable]
public class DeliveryObjective : GameObjective<DeliveryDetail>
{
    [SerializeField] protected DeliveryDetail m_detail = default;

    public override DeliveryDetail Detail => m_detail;
    public virtual void Setup(DeliveryDetail detail) 
    {
        m_detail = detail;
    }
}