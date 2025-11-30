using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputCommandHandler : MonoBehaviour 
{
    [SerializeField] UnityEvent<InputAction.CallbackContext> m_onStarted = default;
    [SerializeField] UnityEvent<InputAction.CallbackContext> m_onPerformed = default;
    [SerializeField] UnityEvent<InputAction.CallbackContext> m_onCanceled = default;
    public void HandleInputAction(InputAction.CallbackContext context) 
    {
        if (context.started)
            m_onStarted?.Invoke(context);
        else if (context.performed)
            m_onPerformed?.Invoke(context);
        else if (context.canceled)
            m_onCanceled?.Invoke(context);
    }


}
