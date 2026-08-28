using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAttackHandler : AttackHandler 
{
    bool m_stop = false;
    public bool StopAttack { get => m_stop; set => m_stop = value; }
    protected Rigidbody2D rb => GetComponent<Rigidbody2D>();
    protected override bool CannotAttack()
    {
        return base.CannotAttack() || StopAttack;
    }
    // get current aiming direction
    public override Vector2 GetAimDirectionNormalized()
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
