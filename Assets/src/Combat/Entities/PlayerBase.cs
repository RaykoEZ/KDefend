using UnityEngine;
using UnityEngine.Events;
public class PlayerBase : Building
{
    [SerializeField] UnityEvent m_onInteract = default;
    public override void Interact()
    {

    }
}
