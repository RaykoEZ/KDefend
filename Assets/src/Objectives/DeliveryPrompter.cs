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
    public static DeliveryDetail None => new DeliveryDetail { };
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

public delegate void OnDeliveryUpdate(DeliveryDetail detail);
// handler UI for delivery objective
public class DeliveryPrompter : MonoBehaviour 
{
    [SerializeField] DeliverySpawner m_spawnBox = default;
    [SerializeField] List<DeliveryHandle> m_currentHandles = default;
    [SerializeField] AudioManager m_sfx = default;

    public event OnObjectiveUpdate<DeliveryDetail> DeliveryReceive;
    // Find the correct Delvery Title
    Predicate<DeliveryHandle> GetIcon(DeliveryDetail obj) => (i) => i.CurrentDelivery.Title == obj.Title;
    // When new delivery lands, point to origin first
    public void NewDelivery(DeliveryDetail newDelivery) 
    {
        m_sfx?.Play("Alert");
        var instance = m_spawnBox?.SpawnDeliveryBox(newDelivery);
        instance.OnDeliveryBegin += OnDeliveryBegin;
    }
    public void OnDeliveryBegin(DeliveryDetail obj) 
    {
        foreach (var item in m_currentHandles)
        {
            if (!item.IsActive)
            {
                m_sfx?.Play("Pickup");
                item.InitDeliveryPickup(obj);
                DeliveryHandle icon = m_currentHandles.Find(GetIcon(obj));
                icon?.BeginDelivery();
                // Remove box
                break;
            }
        }
    }
    public void OnDeliveryComplete(DeliveryDetail obj)
    {
        DeliveryHandle icon = m_currentHandles.Find(GetIcon(obj));
        icon?.ResetHandle();
        m_sfx?.Play("Objective_Complete");
        Debug.Log("Destination reached");
        DeliveryReceive?.Invoke(obj);
    }
}
