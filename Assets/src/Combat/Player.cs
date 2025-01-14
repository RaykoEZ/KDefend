using UnityEngine;
using UnityEngine.InputSystem;

public class Player : BaseCharacter 
{
    [SerializeField] MeleeAttack m_bat = default;
    [SerializeField] BaseProjectile m_testShot = default;
    void Start()
    {
        m_currentWeapon = m_bat;
    }
    // turn off firing
    void Update()
    {
        m_firing = Mouse.current.leftButton.isPressed;
    }
    // Trigger this when player clicks to fire weapon
    public void OnFire() 
    {
        UseWeapon();
    }
    // get current aiming direction
    protected override Vector2 GetAimDirection()
    {
        Vector2 world = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 pos = new Vector2(rb.position.x, rb.position.y);
        return (world - pos).normalized;
    }
}