using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(IMovement))]
public class Dash : ActiveAbility
{
    [SerializeField] bool m_isPlayer = default;
    [Range(0.1f, 100f)]
    [SerializeField] float m_dashStrength = default;
    Vector2 m_direction = Vector2.zero;
    protected Rigidbody2D RB2D => GetComponent<Rigidbody2D>();
    protected IMovement MovementHandle => GetComponent<IMovement>();
    void FixedUpdate()
    {
        if (m_isPlayer) 
        { 
            TrackPlayerCursor();
        }
    }
    public void TrackPlayerCursor() 
    {
        Vector2 world = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        m_direction = (world - pos).normalized;
    }
    protected override void Effect_Internal()
    {
        // get NPC direction if they are dashing
        if (!m_isPlayer) 
        { 
            m_direction = MovementHandle.DirectionNormalized;
        }
        Debug.Log("dash");
        // * 100f as base multiplier
        RB2D?.AddForce(m_direction * m_dashStrength * 100f * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}
