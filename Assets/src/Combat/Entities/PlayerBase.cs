using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PlayerBase : Building
{
    [SerializeField] UnityEvent m_onInteract = default;
    [SerializeField] KDefenderDataSource m_state = default;
    void Start()
    {
        m_current = m_state.Current.CafeValue.Property;
    }
    public override void Interact()
    {

    }
}
