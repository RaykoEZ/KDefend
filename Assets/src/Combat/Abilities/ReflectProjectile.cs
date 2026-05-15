using UnityEngine;

public class ReflectProjectile : ActiveAbility
{
    [SerializeField] float m_duration = default;
    [SerializeField] Transform m_reflectParent = default;
    protected override void Effect_Internal()
    {
        m_reflectParent.gameObject.SetActive(true);
        StartChanneling(m_duration, StopReflect);
    }
    public void OnReflect(Collider2D collision)
    {
        if (collision.attachedRigidbody == null) return;
        // when projectile hit this body, trigger on hit effects from projectile
        if (collision.attachedRigidbody.TryGetComponent(out BaseProjectile projectile))
        {
            projectile?.Reflect(gameObject.layer);
        }
    }
    public void StopReflect() 
    {
        m_reflectParent.gameObject.SetActive(false);
    }
}
