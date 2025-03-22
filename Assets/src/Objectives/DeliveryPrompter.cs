using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "deli_", menuName = "New Delivery Droptable", order = 0)]
public class DeliveryDropTable : ScriptableObject
{
    [SerializeField] List<DeliveryDetail> m_dropList = default;
    public List<DeliveryDetail> DropList { get => m_dropList; }
}
[Serializable]
public struct DeliveryDetail 
{
    public int Rank;
    public int DestinationIndex;
    public DeliveryType DeliverType;
    // affects drop chance
    public float DropChanceWeight;
    public string Title;
    [TextArea(5, 10)]
    public string Description;
}
public delegate void OnTimeOut<T>(T sender);
public class DeliveryPrompter : MonoBehaviour 
{
    [SerializeField] List<DeliveryIcon> m_currentIcons = default;
    public void NewDelivery(DeliveryObjective newDelivery) 
    { 
    }
    public void OnDeliverySuccess() 
    { 

    }
    public void OnDeliveryFail() 
    { 
    
    }
}
