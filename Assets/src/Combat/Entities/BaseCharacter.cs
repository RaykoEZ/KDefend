using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Anything that can be pushed away by a force
public interface IPushable
{
    public void Push(Vector2 dir, float power);
}

public class BaseCharacter : BaseEntity , IPushable
{
    [SerializeField] protected AttackHandler m_attackHandler = default;
    public virtual IMovement Movement { get; }
    public virtual AttackHandler AttackHandle => m_attackHandler;
    public void Push(Vector2 dir, float power)
    {
        if (Mathf.Approximately(CurrentStats.Property.KnockbackModifier, 0f)) return;
        float mod = CurrentStats.Property.KnockbackModifier * power;
        StartCoroutine(Push_Internal(dir, mod));
    }
    protected virtual IEnumerator Push_Internal(Vector2 dirNormalize, float power)
    {
        GetComponent<Rigidbody2D>()?.AddForce(dirNormalize * power, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
    }
}
