using System;
using UnityEngine;
using Curry.Events;
[Serializable]
public class DeliveryObjective : GameObjective
{
    [SerializeField] private int m_rank = default;
    [SerializeField] private int m_destinationIndex = default;
    public int Rank { get => m_rank; }
    public int DestinationIndex { get => m_destinationIndex; }
    public virtual void Setup(DeliveryDetail detail) 
    {
        m_title = detail.Title;
        m_description = detail.Description;
        m_rank = detail.Rank;
        m_destinationIndex = detail.DestinationIndex;
    }
}
