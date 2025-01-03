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
    void Update()
    {
        RB2D?.AddForce(m_controlling.BaseStats.MoveSpeed * m_movementDirection * Time.deltaTime, ForceMode2D.Impulse);
    }
}
