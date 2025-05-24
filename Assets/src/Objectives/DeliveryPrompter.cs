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
    public static List<string> GetTitleList(List<DeliveryDetail> toGet) 
    {
        List<string> ret = new List<string>();
        foreach (var item in toGet)
        {
            ret.Add(item.Title);
        }
        return ret;
    }
}

public delegate void OnDeliveryUpdate(DeliveryBox detail);
// handler UI for delivery objective
public class DeliveryPrompter : MonoBehaviour 
{
    [SerializeField] DeliverySpawner m_spawnBox = default;
    [SerializeField] List<DeliveryHandle> m_currentHandles = default;
    public event OnObjectiveUpdate<DeliveryDetail> DeliveryReceive;
    // Find the correct Delvery Title
    Predicate<DeliveryHandle> GetIcon(DeliveryDetail obj) => (i) => i.CurrentDelivery.Title == obj.Title;
    // When new delivery lands, point to origin first
    public void NewDelivery(DeliveryDetail newDelivery) 
    {
        var instance = m_spawnBox?.SpawnDeliveryBox(newDelivery);
        instance.OnDeliveryBegin += OnDeliveryBegin;
    }
    public void OnDeliveryBegin(DeliveryBox obj) 
    {
        foreach (var item in m_currentHandles)
        {
            if (item.IsActive)
            {
                item.InitDeliveryPickup(obj.Detail);
                DeliveryHandle icon = m_currentHandles.Find(GetIcon(obj.Detail));
                icon?.BeginDelivery();
                obj.OnDeliveryReceive += OnDeliveryComplete;
                // Remove box
                obj?.Hide();
                break;
            }
        }
    }
    public void OnDeliveryComplete(DeliveryBox obj)
    {
        DeliveryHandle icon = m_currentHandles.Find(GetIcon(obj.Detail));
        icon?.ResetHandle();
        DeliveryReceive?.Invoke(obj.Detail);
        // despawn box
        Destroy(obj.gameObject);
    }
}
