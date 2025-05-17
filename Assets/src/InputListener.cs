using UnityEngine;

public class InputListener : MonoBehaviour 
{
    [SerializeField] TemporaryInputAction m_inputToListen = default;
    public void Enable()
    {
        m_inputToListen?.Enable();
    }
    public virtual void Disable()
    {
        m_inputToListen?.Disable();
    }
}