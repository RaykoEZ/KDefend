using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

[Serializable]
public struct EntityProperty
{
    public int Health;
    [Range(0f, 200f)]
    public float MoveSpeed;
}
// negative for damage
// positive for healing
public delegate void OnHpUpdate(int change);
public class BaseEntity : MonoBehaviour 
{
    [SerializeField] protected EntityProperty m_base;
    [SerializeField] protected UnityEvent<BaseEntity> m_onDefeat = default;
    [SerializeField] protected UnityEvent<int> m_onTakeDamage = default;
    protected EntityProperty m_current;
    public event OnHpUpdate OnTakeDamage;
    public event OnHpUpdate OnHeal;
    protected Rigidbody2D rb => GetComponent<Rigidbody2D>();
    public EntityState BaseStats => new EntityState
    {
        Property = m_base,
        Position = transform.position
    };
    public EntityState CurrentStats { 
        get => new EntityState { 
            Property = m_current,
            Position = transform.position};}
    protected virtual void Awake()
    {
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
    public virtual void Init(EntityState state) 
    {
        transform.position = state.Position;
        m_current = state.Property;
    }
    public void ModifySpeed(float mod) 
    {
        float change = Mathf.Abs(mod) * m_base.MoveSpeed;
        if (mod > 0f) 
        {
            m_current.MoveSpeed = Mathf.Min(3f, m_current.MoveSpeed + change);
        }
        else 
        {
            m_current.MoveSpeed = Mathf.Max(0f, m_current.MoveSpeed - change);
        }
    }
    public void Heal(int heal) 
    {
        heal = Mathf.Abs(heal);
        m_current.Health += heal;
        OnHeal?.Invoke(heal);
    }
    public virtual void TakeDamage(int baseDamage) 
    {
        m_current.Health -= baseDamage;
        m_onTakeDamage?.Invoke(-baseDamage);
        OnTakeDamage?.Invoke(-baseDamage);
        if (CurrentStats.Property.Health <= 0f) 
        {
            OnDefeat();
        }
    }
    protected virtual void OnDefeat() 
    {
        m_onDefeat?.Invoke(this);
    }
}