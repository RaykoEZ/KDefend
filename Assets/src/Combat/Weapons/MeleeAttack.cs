using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class MeleeAttack : BaseWeapon
{
    [SerializeField] protected PlayableDirector m_director = default;
    [SerializeField] protected PlayableAsset m_attackPattern = default;
    // determine repeat inputs during attack animation
    private bool inProgress = false;
    private bool comboFrame = false;
    public override bool InstantiateWeapon => false;
    public virtual bool ComboFrame { protected get => comboFrame; set => comboFrame = value; }
    public virtual bool InProgress { protected get => inProgress; set => inProgress = value; }
    Vector2 m_currentDirection = Vector2.zero;
    protected override void LaunchAttack(Vector2 directionNormalized)
    {
        if (InProgress && !ComboFrame)
        {
            return;
        }
        // determine combo behaviour
        InProgress = true;
        if (ComboFrame) 
        {
            OnCombo();
        }
        else 
        {
            m_currentDirection = directionNormalized;
            m_director.Play(m_attackPattern);
        }
    }
    protected virtual void OnCombo() 
    {
        ComboFrame = false;
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