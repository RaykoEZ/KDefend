using Curry.Events;
using System.Collections.Generic;
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
    Rigidbody2D RB2D => GetComponent<Rigidbody2D>();
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
    }
    void FixedUpdate()
    {
        s_playerPosition = transform.position;
        if (m_movable)
        {
            RB2D?.AddForce(m_controlling.CurrentStats.Property.MoveSpeed *
                m_movementDirection * Time.fixedDeltaTime, ForceMode2D.Impulse);
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
