using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PlayerBase : Building
{
    [SerializeField] UnityEvent m_onInteract = default;
    public override void Interact()
    {
        m_onInteract?.Invoke();
    }
    public virtual void Init(KDefenderGameState state) 
    {
        m_current = state.CafeValue.Property;
    }
}
