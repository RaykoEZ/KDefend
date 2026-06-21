using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class MeleeAttack : BaseWeapon
{
    [SerializeField] protected PlayableDirector m_director = default;
    [SerializeField] protected PlayableAsset m_attackPattern = default;
    // determine repeat inputs during attack animation
    private bool inProgress = false;
    public override bool InstantiateWeapon => false;
    public virtual bool InProgress { protected get => inProgress; set => inProgress = value; }
    public override void LaunchAttack(Vector2 directionNormalized)
    {
        if (InProgress)
        {
            return;
        }
        // determine combo behaviour
        InProgress = true;
        GameUtil.PlaySequence(m_director, m_attackPattern);
    }
}