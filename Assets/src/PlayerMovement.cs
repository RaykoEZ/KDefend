using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Player m_controlling = default;
    Vector2 m_movementDirection = Vector2.zero;
    static Vector2 s_moveDirection = Vector2.zero;
    static Vector2 s_playerPosition = Vector2.zero;
    public static Vector2 PlayerMovementDirection => s_moveDirection;
    public static Vector2 PlayerPosition => s_playerPosition;
    Rigidbody2D RB2D => GetComponent<Rigidbody2D>();


    public void OnMove(InputAction.CallbackContext value)
    {
        m_movementDirection = value.ReadValue<Vector2>();
        // update static direction report
        s_moveDirection = m_movementDirection;
    }
    void FixedUpdate()
    {
        s_playerPosition = transform.position;
        RB2D?.AddForce(m_controlling.CurrentStats.Property.MoveSpeed * 
            m_movementDirection * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}
