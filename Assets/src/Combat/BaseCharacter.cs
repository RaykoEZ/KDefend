using UnityEngine;

public class BaseCharacter : BaseEntity 
{
    protected IWeapon m_currentWeapon;
    public virtual void FireWeapon(Vector2 directionNormalized) 
    {
        m_currentWeapon.Fire(directionNormalized);
    }
}
