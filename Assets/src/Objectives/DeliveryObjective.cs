using System;
using UnityEngine;
using Curry.Events;

[Serializable]
public class DeliveryObjective : GameObjective
{
    [SerializeField] private int m_bonusTimeLimit = default;
    [SerializeField] private int m_destinationIndex = default;
    public int BonusTimeLimit { get => m_bonusTimeLimit; }
    public int DestinationIndex { get => m_destinationIndex; }
}
