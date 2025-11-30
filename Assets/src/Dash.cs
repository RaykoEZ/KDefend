using UnityEngine;
using UnityEngine.InputSystem;
// Dash action - an impulse towards the direction of travel
[RequireComponent(typeof(BaseEntity), typeof(Rigidbody2D), typeof(IMovement))]
public class Dash : ActiveAbility
{
    [Range(0.1f, 100f)]
    [SerializeField] protected float m_dashStrength = default;
    protected Vector2 m_direction = Vector2.zero;
    protected BaseEntity User => GetComponent<BaseEntity>();
    protected Rigidbody2D RB => GetComponent<Rigidbody2D>();
    protected IMovement MovementHandle => GetComponent<IMovement>();
    public void TrackPlayerCursor() 
    {
        Vector2 world = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        m_direction = (world - pos).normalized;
    }
    protected override void Effect_Internal()
    {
        // get NPC direction if they are dashing
        m_direction = MovementHandle.DirectionNormalized;
        // * 100f as base multiplier
        RB?.AddForce(m_direction * m_dashStrength * 100f * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}
