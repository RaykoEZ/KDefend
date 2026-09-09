using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DialogueLua_OnPlayerDeath : MonoBehaviour 
{
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    public bool unregisterOnDisable = false;
    string m_spawnFunctionID => nameof(OnPlayerDeath);

    void OnEnable()
    {
        // Make spawn function ID bound to each unique spawn wave 
        Lua.RegisterFunction(m_spawnFunctionID, this, SymbolExtensions.GetMethodInfo(() => OnPlayerDeath()));
    }
    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction(m_spawnFunctionID);
        }
    }
    public void OnPlayerDeath() 
    {
        int deaths = DialogueLua.GetVariable("NumDeaths").AsInt;
        DialogueLua.SetVariable("NumDeaths", deaths++);
    }
}
/**/