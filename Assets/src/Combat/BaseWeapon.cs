using Unity.Properties;
using UnityEngine;
public abstract class BaseWeapon : MonoBehaviour, IHitsEntity
{
    [SerializeField] protected WeaponProperty m_property = default;
    // For bullets we instantiate bullets on attack, for melee, don't instantiate
    public abstract bool InstantiateWeapon { get; }
    public virtual WeaponProperty Property => m_property;
    protected static T NewAttackInstance<T>(T prefabRef, Transform parent) where T : BaseWeapon
    {
        return Instantiate(prefabRef, parent);
    }
    // call to fire off a projectile
    public static void Attack<T>(T weaponRef, Transform parent, Vector2 directionNormalized, bool instamtiate = true) where T : BaseWeapon
    {
        T instance = instamtiate? NewAttackInstance(weaponRef, parent) : weaponRef;
        instance?.LaunchAttack(directionNormalized);
    }
    public virtual void OnHit<T>(T hit) where T : BaseEntity
    {
        hit?.TakeDamage(Property.Damage);
    }
    protected abstract void LaunchAttack(Vector2 directionNormalized);
}
