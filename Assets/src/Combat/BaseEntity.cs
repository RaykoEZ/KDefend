using System;
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
    protected EntityProperty m_current;
    protected Rigidbody2D rb => GetComponent<Rigidbody2D>();
    public EntityProperty BaseProperty => m_base;
    void Awake()
    {
        m_current = m_base;
    }
    public virtual void TakeDamage(int baseDamage) 
    {
        m_current.Health -= baseDamage;
        if (m_current.Health <= 0f) 
        {
            OnDefeat();
        }
    }
    protected virtual void OnDefeat() 
    {
        m_onDefeat?.Invoke(this);
    }
}