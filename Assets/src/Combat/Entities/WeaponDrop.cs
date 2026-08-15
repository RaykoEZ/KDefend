using UnityEngine;
// add weapon to player arsenal
public class WeaponDrop : MonoBehaviour 
{
    [SerializeField] BaseWeapon m_weaponRef = default;
    public void PickupWeapon(Player player)
    {
        player?.GetComponent<AttackHandler>()?.AddWeapon(m_weaponRef);
    }
}
