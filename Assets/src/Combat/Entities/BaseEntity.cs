using System;
using Curry.Explore;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
[Serializable]
public struct EntityProperty
{
    public int Health;
    [Range(0f, 2f)]
    public float KnockbackModifier;
    [Range(0f, 200f)]
    public float MoveSpeed;
    [Range(0f, 99999f)]
    public float AtkModifier;
}
// negative for damage
// positive for healing
public delegate void OnHpUpdate(int change);
[RequireComponent(typeof(AudioSource))]
public class BaseEntity : MonoBehaviour 
{
    [SerializeField] protected EntityProperty m_base;
    [SerializeField] protected UnityEvent<BaseEntity> m_onDefeat = default;
    [SerializeField] protected UnityEvent<int> m_onTakeDamage = default;
    bool m_isInvincible;
    protected EntityProperty m_current;
    public event OnHpUpdate OnTakeDamage;
    public event OnHpUpdate OnHeal;
    public bool IsInvincible { get => m_isInvincible; set => m_isInvincible = value; }
    public EntityState BaseStats => new EntityState
    {
        Property = m_base,
        Position = transform.position,

    };
    public EntityState CurrentStats { 
        get => new EntityState { 
            Property = m_current,
            Position = transform.position,
        };
    }
    public float HpRatio => CurrentStats.Property.Health / (float)m_base.Health;
    // for testing stats
#if UNITY_EDITOR
    void Awake() 
    {
        m_current = m_base;
    }
#endif
    public virtual void OnHit(Collider2D collision)
    {
        if (collision.attachedRigidbody == null) return;
        // when projectile hit this body, trigger on hit effects from projectile
        if (collision.attachedRigidbody.TryGetComponent(out IHitsEntity result))
        {
            // trigger on hit events
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
            m_current.MoveSpeed = Mathf.Min(3f * m_base.MoveSpeed, m_current.MoveSpeed + change);
        }
        else 
        {
            m_current.MoveSpeed = Mathf.Max(0f, m_current.MoveSpeed - change);
        }
    }
    public void ModifyKnockback(float mod)
    {
        float change = Mathf.Abs(mod) * m_base.KnockbackModifier;
        if (mod > 0f)
        {
            m_current.KnockbackModifier = Mathf.Min(2.5f, m_current.KnockbackModifier + change);
        }
        else
        {
            m_current.KnockbackModifier = Mathf.Max(0f, m_current.KnockbackModifier - change);
        }
    }
    public void SetBaseHp(int value) 
    {
        if (value <= 0) return;
        m_base.Health = value;
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