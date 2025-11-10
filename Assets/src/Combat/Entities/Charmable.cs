using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Charmable : MonoBehaviour
{
    Enemy Control => GetComponent<Enemy>();

    public void Charm(float duration) 
    {
        if (Control == null || duration < 0f) return;
        StopAllCoroutines();
        StartCoroutine(Charm_Internal(duration));
    }
    protected virtual IEnumerator Charm_Internal(float duration) 
    {
        var npc = Control.AttackHandle as NpcAttackHandler;
        npc?.SetTargetDetectAll(true);
        yield return new WaitForSeconds(duration);
        npc?.SetTargetDetectAll(false);
    }
}
