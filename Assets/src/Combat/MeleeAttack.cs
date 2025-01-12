using UnityEngine;
using UnityEngine.Playables;

public class MeleeAttack : BaseWeapon
{
    [SerializeField] protected Transform m_hitboxRoot = default;
    [SerializeField] protected PlayableDirector m_director = default;
    [SerializeField] protected PlayableAsset m_attackPattern = default;
    protected override void LaunchAttack(Vector2 directionNormalized)
    {
        GameUtil.AimTowards2D(m_hitboxRoot, directionNormalized);
        m_director.Play(m_attackPattern);
    }
}