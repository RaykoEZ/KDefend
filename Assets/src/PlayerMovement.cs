using Curry.Events;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour, IMovement
{
    [SerializeField] Player m_controlling = default;
    bool m_movable = true;
    Vector2 m_movementDirection = Vector2.zero;
    static Vector2 s_moveDirection = Vector2.zero;
    static Vector2 s_playerPosition = Vector2.zero;
    public static Vector2 PlayerMovementDirection => s_moveDirection;
    public static Vector2 PlayerPosition => s_playerPosition;
    public Vector2 DirectionNormalized => PlayerMovementDirection;
    public Vector2 Position => PlayerPosition;
    Rigidbody2D RB2D => m_controlling?.GetComponent<Rigidbody2D>();
    Animator Anim => m_controlling?.GetComponent<Animator>();

    void OnEnable()
    {
        KDEventHandler.ListenToGlobal(GameEventTriggerType.PauseGame, OnPause);
    }
    void OnDisable()
    {
        KDEventHandler.UnlistenFromGlobal(GameEventTriggerType.PauseGame, OnPause);
    }
    protected void OnPause(object sender, KDEventInfo args) 
    {
        // check toggle sender, if sender arg  are null, keep control
        if (args == null || args.Payload == null) return;
        if (args.Payload.TryGetValue("isOn", out object result) && result is bool isGameActive)
        {
            // if we were stopped before, don't allow movement before
            m_movable = isGameActive;
        }
    }
    
    public void OnMove(InputAction.CallbackContext value)
    {
        m_movementDirection = value.ReadValue<Vector2>();
        // update static direction report
        s_moveDirection = m_movementDirection;
        Anim?.SetBool("isMoving", m_movementDirection.sqrMagnitude > 0f);
        if (m_movementDirection.x < 0f)
        {
            Anim?.SetTrigger("faceLeft");
        }
        // face forawrd when neither left or right
        else if (Mathf.Approximately(m_movementDirection.x, 0f)) 
        {
            Anim?.SetTrigger("faceForward");
        }
        else
        {
            Anim?.SetTrigger("faceRight");
        }
    }
    void FixedUpdate()
    {
        s_playerPosition = transform.position;
        if (m_movable)
        {
            Vector2 force = m_controlling.CurrentStats.Property.MoveSpeed *
                m_movementDirection * Time.fixedDeltaTime;
            RB2D?.AddForce(force, ForceMode2D.Impulse);
        }

    }
    
    // for pausing movement
    public void StartMoving()
    {
        m_movable = true;
    }

    public void StopMoving()
    {
        m_movable = false;
    }
}
