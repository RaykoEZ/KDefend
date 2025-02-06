using System.Collections;
using UnityEngine;
public abstract class BaseWeapon : MonoBehaviour, IHitsEntity
{
    [SerializeField] protected WeaponProperty m_weaponProperty = default;
    protected Coroutine m_attack;
    // For bullets we instantiate bullets on attack, for melee, don't instantiate
    protected bool firing = false;
    public abstract bool InstantiateWeapon { get; }
    public bool Firing => firing;
    public virtual WeaponProperty WeaponProperty => m_weaponProperty;
    protected Vector2 m_currentDirection = Vector2.zero;
    protected static T NewAttackInstance<T>(T prefabRef, Transform parent) where T : BaseWeapon
    {
        return Instantiate(prefabRef, parent);
    }
    protected IEnumerator AttackSequence<T>(T weaponRef, Transform parent, Vector2 directionNormalized, bool instantiate = true)
    where T: BaseWeapon
    {
        // shoot a fire cycle
        for (int i = 0; i < WeaponProperty.AttackPerCycle; i++)
        {
            // play behaviour for each attack instance (e.g. a swing of a bat/a bullet flying)
            T instance = instantiate ? NewAttackInstance(weaponRef, parent) : weaponRef;
            m_currentDirection = directionNormalized;
            instance?.LaunchAttack(directionNormalized);
            yield return new WaitForSeconds(WeaponProperty.DelayPerAttack);
        }
    }
    // call to fire off a projectile
    public IEnumerator Attack<T>(T weaponRef, Transform parent, Vector2 directionNormalized, bool instantiate = true) 
    where T : BaseWeapon
    {
        yield return AttackSequence(weaponRef, parent, directionNormalized, instantiate);
    }
    public virtual void OnHit<T>(T hit) where T : BaseEntity
    {
        hit?.TakeDamage(WeaponProperty.Damage);
        if (hit is IPushable push)
        {
            push.Push(m_currentDirection, WeaponProperty.PushPower);
        }
    }
    protected abstract void LaunchAttack(Vector2 directionNormalized);
}
