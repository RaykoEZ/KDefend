using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
// Listens to an input action and trigger an event,
// unlistens after triggering once auto/ manual cancel
[Serializable]
public class TemporaryInputAction 
{
    [SerializeField] bool m_autoDisable = default;
    [SerializeField] InputActionReference m_inputTarget = default;
    [SerializeField] UnityEvent<InputAction.CallbackContext> m_triggerOnAction;
    bool m_enabled = false;
    public virtual void Enable()
    {
        if (m_enabled) return;
        m_enabled = true;
        m_inputTarget.action.performed += Trigger;
    }
    public virtual void Disable()
    {
        m_inputTarget.action.performed -= Trigger;
        m_enabled = false;
    }
    protected virtual void Trigger(InputAction.CallbackContext c) 
    {
        if (m_autoDisable) 
        {
            Disable();
        }
        m_triggerOnAction?.Invoke(c);
    }
}
