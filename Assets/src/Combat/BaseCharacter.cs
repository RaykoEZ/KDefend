using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
// Anything that can be pushed away by a force
public interface IPushable
{
    public void Push(Vector2 dir, float power);
}
public class BaseCharacter : BaseEntity , IPushable
{
    protected bool m_firing = false;
    protected BaseWeapon m_currentWeapon;
    protected Coroutine m_weaponCall;
    protected virtual Vector2 GetAimDirection()
    {
        return Vector2.up;
    }
    public virtual void UseWeapon() 
    {
        if (!m_firing) 
        {
            m_firing = true;
            m_weaponCall = StartCoroutine(Fire_Internal());
        }
    }
    IEnumerator Fire_Internal() 
    {
        int numShots = m_currentWeapon.Property.AttackPerCycle;
        while (m_firing)
        {
            // shoot a fire cycle
            for (int i = 0; i < numShots; i++)
            {
                BaseWeapon.Attack(m_currentWeapon, transform, GetAimDirection(), m_currentWeapon.InstantiateWeapon);
                yield return new WaitForSeconds(m_currentWeapon.Property.DelayPerAttack);
            }
            // next firing cycle
            yield return new WaitForSeconds(m_currentWeapon.Property.DelayPerCycle);
        }
        m_firing = false;
        m_weaponCall = null;
    }

    public void Push(Vector2 dir, float power)
    {
        StartCoroutine(Push_Internal(dir, power));
    }
    protected virtual IEnumerator Push_Internal(Vector2 dirNormalize, float power)
    {
        GetComponent<Rigidbody2D>()?.AddForce(dirNormalize * power, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
    }
}
