using System.Collections;
using UnityEngine;
using UnityEngine.AI;
// base enemy behaviour
public delegate void OnEnemyUpdate(Enemy toUpdate);
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : BaseCharacter, IHitsEntity
{
    [SerializeField] int m_type = default;
    [SerializeField] int m_contactDamage = default;
    [Range(-100, 100)]
    [SerializeField] protected int m_threatIncrease = default;
    [SerializeField] protected RangeDetector m_targeting = default;
    protected float m_speedVariant;
    private Vector2 m_defaultTarget;
    protected Vector2 m_currentDestination;
    protected BaseEntity m_target;
    Coroutine m_movement;
    Coroutine m_attack;
    public int ThreatIncrease => m_threatIncrease;
    public BaseEntity CurrentTarget { get => m_target; }
    public NavMeshAgent Navigator => GetComponent<NavMeshAgent>();
    public EnemyState State => 
        new EnemyState { 
            EnemyIndex = m_type, 
            State = CurrentStats };

    public event OnEnemyUpdate OnDefeated;
    protected virtual void Update() 
    {
        if (m_attack == null && m_targeting.IsInRange(m_target)) 
        {
            UseWeapon();
        }
        else 
        {
            m_keepFiring = false;
        }
    }
    protected override Vector2 GetAimDirection()
    {
        return m_target == null? Vector2.zero : (m_target.transform.position - transform.position).normalized;
    }
    void SetupNavigation() 
    {
        var nav = Navigator;
        nav.updateRotation = false;
        nav.updateUpAxis = false;
        nav.enabled = true;
    }
    public void Init(BaseEntity defaultTarget = null)
    {
        base.Init(BaseStats);
        EnemyAggroHandler.Add(this);
        m_defaultTarget = defaultTarget == null? transform.position : defaultTarget.transform.position;
        m_target = defaultTarget;
        m_speedVariant = Random.Range(0.75f, 1.25f);
        SetupNavigation();
        StartMoving();
    }
    public void SetAggro(bool enable = true) 
    {
        m_targeting.enabled = enable;
    }
    public void UpdateTarget(BaseEntity newTarget) 
    {
        if (newTarget == null) return;
        m_target = newTarget;
        SetupNavigation();
        // if not moving, start chasing
        StartMoving();
    }
    public void StartMoving()
    {
        if (m_movement == null)
        {
            Navigator.enabled = true;
            Navigator.isStopped = false;
            m_movement = StartCoroutine(Movement());
        }
    }
    public void StopMoving()
    {
        if (m_movement != null)
        {
            StopCoroutine(m_movement);
            Navigator.velocity = Vector3.zero;
            Navigator.isStopped = true;
            m_movement = null;
            Navigator.enabled = false;
        }
    }
    public virtual void ResetTarget()
    {
        float duration = UnityEngine.Random.Range(1f, 5f);
        StartCoroutine(Standby(duration));
    }
    protected virtual IEnumerator Standby(float duration) 
    {
        StopMoving();
        yield return new WaitForSeconds(duration);
        m_target = null;
    }
    public override void TakeDamage(int baseDamage)
    {
        // the lower the enemy hp, the greater the stun duration
        float stunDuration = UnityEngine.Random.Range(0.3f, 0.5f);
        StartCoroutine(HitStun(stunDuration));
        base.TakeDamage(baseDamage);
    }
    protected override void OnDefeat()
    {
        StartCoroutine(Defeat_Internal());
    }
    protected virtual IEnumerator Defeat_Internal() 
    {
        StopMoving();
        base.OnDefeat();
        OnDefeated?.Invoke(this);
        yield return new WaitForSeconds(0.5f);
        Despawn();
    }
    public void Despawn() 
    {
        StopMoving();
        EnemyAggroHandler.Remove(this);
        Destroy(gameObject);
    }
    protected virtual IEnumerator HitStun(float duration) 
    {
        StopMoving();
        m_keepFiring = false;
        yield return new WaitForSeconds(duration);
        StartMoving();
        yield return new WaitForSeconds(duration);
        UseWeapon();
    }
    protected virtual IEnumerator Movement()
    {
        var nav = Navigator;
        m_currentDestination = m_target == null ? m_defaultTarget : m_target.transform.position;
        nav?.SetDestination(m_currentDestination);
        float dist = Vector2.Distance(transform.position, m_currentDestination);
        float randInterval = Random.Range(0.1f, 0.3f);
        float waitTime;
        while (dist > nav.stoppingDistance)
        {
            // the farther we are from target, the longer our path refresh interval
            waitTime = randInterval * Mathf.Clamp(dist / 250f, 1f, 100f);
            nav?.SetDestination(m_currentDestination);
            yield return new WaitForSeconds(waitTime);
            m_currentDestination = m_target == null? m_defaultTarget : m_target.transform.position;
            dist = Vector2.Distance(transform.position, m_currentDestination);
            Debug.Log(dist);
        }
        m_movement = null;
    }
    public override void UseWeapon()
    {
        if (m_currentWeapons.Count == 0) return;
        if (m_attack == null)
        {
            m_keepFiring = true;
            m_attack = StartCoroutine(AttackCycle_Sequence());
        }
    }
    // make attack pattern sequential vs silmultaneous
    IEnumerator AttackCycle_Sequence() 
    {
        while (m_keepFiring)
        {
            for (int i = 0; i < m_currentWeapons.Count; i++)
            {
                // unleash one instance of a weapon's attack, include its recovery frames
                yield return Attack_Internal(m_currentWeapons[i]);
            }
        }
        m_attack = null;
    }
    // contact damage
    public virtual void OnHit<T>(T hit) where T : BaseEntity
    {
        if ( hit is Player || hit is PlayerBase)
        {
            hit?.TakeDamage(m_contactDamage);
        }
        Vector2 dir = hit.transform.position - transform.position;
        if (hit is IPushable push && !(hit is Enemy))
        {
            push.Push(dir.normalized, 0.25f);
            Push(-dir.normalized, 0.25f);
            float stunDuration = Random.Range(0.5f, 1f);
            StartCoroutine(HitStun(stunDuration));
        }
    }
}