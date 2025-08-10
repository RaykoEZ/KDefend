using UnityEngine;
using UnityEngine.Events;

public abstract class Building : MonoBehaviour 
{
    [SerializeField] UnityEvent m_onInteract = default;
    public virtual void OnPlayerInteract() 
    {
        m_onInteract?.Invoke();
        Interact_Internal();
    }
    protected abstract void Interact_Internal();
}