using UnityEngine;

public class HitDetection : MonoBehaviour, IHitsEntity
{
    [SerializeField] BaseWeapon m_weapon = default;
    public void OnHit<T>(T hit) where T : BaseEntity
    {
        m_weapon?.OnHit(hit);
    }
}
