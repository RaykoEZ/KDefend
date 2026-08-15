using UnityEngine;
using PixelCrushers.DialogueSystem;
// Event for Raising flags only for Diaglogue System toolkit
public class DialogueLua_RaiseFlag : MonoBehaviour
{
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    public bool unregisterOnDisable = false;
    [Tooltip("New event flag for this spawn event")]
    [SerializeField] KD_StaticEventFlags m_raiseEventFlag = default;
    [Tooltip("Flag Handler Reference in Scene")]
    [SerializeField] StaticFlagEventHandler m_flagHandler = default;
    void OnEnable()
    {
        // Make spawn function ID bound to each unique spawn wave 
        Lua.RegisterFunction(nameof(AppendFlags), this, SymbolExtensions.GetMethodInfo(() => AppendFlags()));
        Lua.RegisterFunction(nameof(SetFlags), this, SymbolExtensions.GetMethodInfo(() => SetFlags()));
    }
    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction(nameof(AppendFlags));
            Lua.UnregisterFunction(nameof(SetFlags));
        }
    }
    public void AppendFlags()
    {
        m_flagHandler?.AppendFlag(m_raiseEventFlag);
    }
    public void SetFlags() 
    {
        m_flagHandler?.SetFlags(m_raiseEventFlag);

    }
}