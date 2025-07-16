using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAttackHandler : AttackHandler 
{
    protected Rigidbody2D rb => GetComponent<Rigidbody2D>();
    // get current aiming direction
    public override Vector2 GetAimDirection()
    {
        Vector2 world = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 pos = new Vector2(rb.position.x, rb.position.y);
        return (world - pos).normalized;
    }
    void Update()
    {
        m_keepFiring = Mouse.current.leftButton.isPressed;
    }
}
