using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseWeapon : MonoBehaviour, IHitsEntity
{
    // when attacking, the spawned instance will be released from the parent given
    [SerializeField] bool m_detachFromUser = default;
    [SerializeField] protected WeaponProperty m_weaponProperty = default;
    [SerializeField] AudioClip m_onLaunchSfx = default;
    [SerializeField] List<EffectModule> m_onHitEffects = default;
    [SerializeField] UnityEvent<BaseEntity> m_onHitCallbacks = default;
    protected Coroutine m_attack;
    // For bullets we instantiate bullets on attack, for melee, don't instantiate
    protected bool firing = false;
    public bool DetachFromUser => m_detachFromUser;
    public abstract bool InstantiateWeapon { get; }
    public bool Firing => firing;
    public virtual WeaponProperty WeaponProperty => m_weaponProperty;
    protected Vector2 m_currentDirection = Vector2.zero;
    public Vector2 CurrentDirection { get => m_currentDirection; set => m_currentDirection = value; }

    // create a new weapon object, for projectiles & summons
    protected static T NewAttackInstance<T>(T prefabRef, Transform instanceParent) where T : BaseWeapon
    {
        Transform parent = prefabRef.DetachFromUser ? instanceParent.parent : instanceParent;
        T ret = Instantiate(prefabRef, parent);
        ret.transform.position = instanceParent.position;
        return ret;
    }
    protected IEnumerator AttackSequence<T>(T weaponRef, Transform instanceParent, Vector2 directionNormalized, bool instantiate = true)
    where T: BaseWeapon
    {
        // shoot a fire cycle
        for (int i = 0; i < WeaponProperty.AttackPerCycle; i++)
        {
            // play behaviour for each attack instance (e.g. a swing of a bat/a bullet flying)
            T instance = instantiate ? NewAttackInstance(weaponRef, instanceParent) : weaponRef;
            m_currentDirection = ModifyAttackDirection(directionNormalized);
            instance?.LaunchAttack(m_currentDirection);
            if (m_onLaunchSfx != null) 
            {
                instance?.GetComponent<AudioSource>()?.PlayOneShot(m_onLaunchSfx);
            }
            yield return new WaitForSeconds(WeaponProperty.DelayPerAttack);
        }
    }
    // used for changing firing/attacking direction
    protected virtual Vector2 ModifyAttackDirection(Vector2 directionNormalized) 
    {
        return directionNormalized;
    }
    // call to fire off a projectile
    public IEnumerator Attack<T>(T weaponRef, Transform instanceParent, Vector2 directionNormalized, bool instantiate = true) 
    where T : BaseWeapon
    {
        yield return AttackSequence(weaponRef, instanceParent, directionNormalized, instantiate);
    }
    public virtual void OnHit<T>(T hit) where T : BaseEntity
    {
        if (hit.IsInvincible) return;
        Hit_Internal(hit);
    }
    protected void Hit_Internal<T>(T hit) where T : BaseEntity
    {
        hit?.TakeDamage(WeaponProperty.Damage);
        m_onHitCallbacks?.Invoke(hit);
        // trigger on hit effects
        foreach (var item in m_onHitEffects)
        {
            item?.Activate(hit);
        }
        if (hit is IPushable push)
        {
            push.Push(m_currentDirection, WeaponProperty.PushPower);
        }
    }
    public abstract void LaunchAttack(Vector2 directionNormalized);
}
