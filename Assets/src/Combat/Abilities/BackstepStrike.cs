using UnityEngine;
using UnityEngine.AI;
// dashes back and attacks with an attack
public class BackstepStrike : ActiveAbility 
{
    [SerializeField] float m_distance = default;
    [SerializeField] NavMeshAgent m_nav = default;
    [SerializeField] BaseCharacter m_user = default;
    [SerializeField] BaseWeapon m_attack = default;
    protected override void Effect_Internal()
    {
        throw new System.NotImplementedException();
    }
}
