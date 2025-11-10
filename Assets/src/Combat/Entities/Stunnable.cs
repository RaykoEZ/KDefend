using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BaseCharacter))]
public class Stunnable : MonoBehaviour 
{
    BaseCharacter Control => GetComponent<BaseCharacter>();
    public void Stun(float duration) 
    {
        if (Control == null || duration < 0f) return;
        StopAllCoroutines();
        StartCoroutine(HitStun(duration));

    }
    public void Stun()
    {
        float stunDuration = Random.Range(0.1f, 1f);
        Stun(stunDuration);
    }
    protected virtual IEnumerator HitStun(float duration)
    {
        float mod = Control.CurrentStats.Property.KnockbackModifier;
        if (Mathf.Approximately(mod, 0f)) yield break;
        AttackHandler attack = Control.AttackHandle;
        Control?.Movement?.StopMoving();
        attack.KeepFiring = false;
        yield return new WaitForSeconds(duration * mod);
        Control?.Movement?.StartMoving();
        if (attack is NpcAttackHandler npc && npc.AutoAttack)
        {
            npc?.UseWeapon();
        }
    }
}
