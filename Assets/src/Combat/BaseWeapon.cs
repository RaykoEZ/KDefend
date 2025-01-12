using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour, IHitsEntity
{
    [SerializeField] protected WeaponProperty m_property = default;
    public virtual WeaponProperty Property => m_property;
    protected static T NewAttackInstance<T>(T prefabRef, Transform parent) where T : BaseWeapon
    {
        return Instantiate(prefabRef, parent);
    }
    // call to fire off a projectile
    public static void Attack<T>(T prefabRef, Transform parent, Vector2 directionNormalized) where T : BaseWeapon
    {
        T instance = NewAttackInstance(prefabRef, parent);
        instance?.LaunchAttack(directionNormalized);
    }
    public virtual void OnHit<T>(T hit) where T : BaseEntity
    {
        hit?.TakeDamage(Property.Damage);
    }
    protected abstract void LaunchAttack(Vector2 directionNormalized);
}
