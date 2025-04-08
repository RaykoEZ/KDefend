using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] BaseEntity m_controlling = default;
    Vector2 m_movementDirection = Vector2.zero;
    Rigidbody2D RB2D => GetComponent<Rigidbody2D>();
    public void OnMove(InputAction.CallbackContext value)
    {
        m_movementDirection = value.ReadValue<Vector2>();
    }
    void FixedUpdate()
    {
        RB2D?.AddForce(m_controlling.CurrentStats.Property.MoveSpeed * 
            m_movementDirection * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}
