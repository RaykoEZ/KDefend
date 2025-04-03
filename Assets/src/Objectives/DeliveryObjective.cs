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
public class DeliveryObjective : IObjective
{
    [SerializeField] DeliveryDetail m_detail = default;
    public virtual string Title => m_detail.Title;
    public virtual string Description => m_detail.Description;
    public DeliveryDetail Detail { get => m_detail; }
    public event OnObjectiveUpdate OnComplete;
    public event OnObjectiveUpdate OnFail;
    public virtual void Init() { }
    public virtual void Shutdown() { }
    public virtual void Setup(DeliveryDetail detail) 
    {
        m_detail = detail;
    }
    public virtual void Complete()
    {
        OnComplete?.Invoke(this);
    }
    public virtual void Fail()
    {
        OnFail?.Invoke(this);
    }
}