﻿using System;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public struct EntityProperty
{
    public int Health;
    [Range(0f, 10f)]
    public float MoveSpeed;
}
[RequireComponent(typeof(Rigidbody2D))]
public class BaseEntity : MonoBehaviour 
{
    [SerializeField] protected EntityProperty m_base;
    [SerializeField] protected UnityEvent<BaseEntity> m_onDefeat = default;
    [SerializeField] protected UnityEvent<int> m_onTakeDamage = default;
    protected EntityProperty m_current;
    protected Rigidbody2D rb => GetComponent<Rigidbody2D>();
    public EntityProperty BaseStats => m_base;
    public EntityProperty CurrentStats { get => m_current;}
    protected virtual void Awake()
    {
        m_current = m_base;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null) return;
        // when projectile hit this body, trigger on hit effects from projectile
        if (collision.attachedRigidbody.TryGetComponent(out IHitsEntity result))
        {
            result?.OnHit(this);
        }
    }
    public virtual void TakeDamage(int baseDamage) 
    {
        m_current.Health -= baseDamage;
        m_onTakeDamage?.Invoke(baseDamage);
        if (CurrentStats.Health <= 0f) 
        {
            OnDefeat();
        }
    }
    protected virtual void OnDefeat() 
    {
        m_onDefeat?.Invoke(this);
    }
}