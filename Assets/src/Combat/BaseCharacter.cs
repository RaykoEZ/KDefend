using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseCharacter : BaseEntity 
{
    protected bool m_firing = false;
    protected BaseProjectile m_currentWeapon;
    protected Coroutine m_weaponCall;
    public virtual void FireWeapon(Vector2 directionNormalized) 
    {
        if (m_weaponCall == null) 
        {
            m_firing = true;
            m_weaponCall = StartCoroutine(Fire_Internal(directionNormalized));
        }
    }
    IEnumerator Fire_Internal(Vector2 directionNormalized) 
    {
        int numShots = m_currentWeapon.Property.ShotsPerCycle;
        while (m_firing)
        {
            // shoot a fire cycle
            for (int i = 0; i < numShots; i++)
            {
                BaseProjectile.Fire(m_currentWeapon, transform, directionNormalized);
                yield return new WaitForSeconds(m_currentWeapon.Property.DelayPerShot);
            }
            // next firing cycle
            yield return new WaitForSeconds(m_currentWeapon.Property.DelayPerCycle);
        }
        m_weaponCall = null;
    }
}
