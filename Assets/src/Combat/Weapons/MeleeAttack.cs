using System.Collections;
using UnityEngine;
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
    public override void LaunchAttack(Vector2 directionNormalized)
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
            m_director.Play(m_attackPattern);
        }
    }
    protected virtual void OnCombo() 
    {
        ComboFrame = false;
    }
}