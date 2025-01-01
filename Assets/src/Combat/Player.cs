using UnityEngine;
using UnityEngine.InputSystem;

public class Player : BaseCharacter 
{
    // turn off firing
    void Update()
    {
        m_firing = Mouse.current.leftButton.isPressed;
    }
    // Trigger this when player clicks to fire weapon
    public void OnFire() 
    {
        Vector2 world = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 pos = new Vector2(rb.position.x, rb.position.y);
        Vector2 dir = (world - pos).normalized;
        FireWeapon(dir);
    }
}