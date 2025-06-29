using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ReflectProjectile : ActiveAbility
{
    [SerializeField] float m_duration = default;
    protected override void Effect_Internal()
    {
        GetComponent<Collider2D>().enabled = true;
        StartCoroutine(Channeling(m_duration, StopReflect));
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null) return;
        // when projectile hit this body, trigger on hit effects from projectile
        if (collision.attachedRigidbody.TryGetComponent(out BaseProjectile projectile))
        {
            projectile?.Reflect();
        }
    }
    void StopReflect() 
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
