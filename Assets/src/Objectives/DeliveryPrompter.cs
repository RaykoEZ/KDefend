using Curry.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct DeliveryDetail : IObjective
{
    public int OriginIndex;
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
    [SerializeField] DeliverySpawner m_spawnBox = default;
    [SerializeField] List<DeliveryHandle> m_currentIcons = default;
    [SerializeField] List<DirectionPointer> m_pointers = default;

    int m_numActive = 0;
    Predicate<DeliveryHandle> GetIcon(DeliveryDetail obj) => (i) => i.CurrentDelivery.Title == obj.Title;
    // When new delivery lands, point to origin first
    public void NewDelivery(DeliveryDetail newDelivery) 
    {
        if (m_numActive >= m_currentIcons.Count) return;
        m_currentIcons[m_numActive].SpawnPackage(newDelivery);
        var instance = m_spawnBox?.SpawnDeliveryBox(newDelivery);
        instance.OnDeliveryBegin += OnDeliveryBegin;
        m_numActive++;    
    }
    public void OnDeliveryBegin(DeliveryDetail obj) 
    {
        DeliveryHandle icon = m_currentIcons.Find(GetIcon(obj));
        icon?.BeginDelivery();
    }
}
