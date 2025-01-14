using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class MeleeAttack : BaseWeapon
{
    [SerializeField] protected PlayableDirector m_director = default;
    [SerializeField] protected PlayableAsset m_attackPattern = default;
    public override bool InstantiateWeapon => false;
    Vector2 m_currentDirection = Vector2.zero;
    protected override void LaunchAttack(Vector2 directionNormalized)
    {
        m_currentDirection = directionNormalized;
        m_director.Play(m_attackPattern);
    }
    public override void OnHit<T>(T hit)
    {
        base.OnHit(hit);
        if (hit is IPushable push) 
        {
            push.Push(m_currentDirection, Property.PushPower);
        }
    }
}