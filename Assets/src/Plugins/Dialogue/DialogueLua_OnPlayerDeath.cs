using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueLua_OnPlayerDeath : MonoBehaviour 
{
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    public bool unregisterOnDisable = false;
    string m_functionID_death => nameof(OnPlayerDeath);
    void OnEnable()
    {
        // Make spawn function ID bound to each unique spawn wave 
        Lua.RegisterFunction(m_functionID_death, this, SymbolExtensions.GetMethodInfo(() => OnPlayerDeath()));
    }
    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction(m_functionID_death);
        }
    }
    public void OnPlayerDeath() 
    {
        int deaths = DialogueLua.GetVariable("NumDeaths").AsInt;
        DialogueLua.SetVariable("NumDeaths", deaths + 1);
    }
    public void RespawnFlag(bool toSet) 
    {
        DialogueLua.SetVariable("RespawnFlag", toSet);
    }
}
/**/