using System;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DialogueLua_SetUsername : MonoBehaviour
{
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    public bool unregisterOnDisable = false;
    string m_spawnFunctionID => nameof(SetUsername);

    void OnEnable()
    {
        // Make spawn function ID bound to each unique spawn wave 
        Lua.RegisterFunction(m_spawnFunctionID, this, SymbolExtensions.GetMethodInfo(() => SetUsername()));
    }
    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction(m_spawnFunctionID);
        }
    }
    public void SetUsername()
    {
        string result = Environment.UserName;
        DialogueLua.SetVariable("Username", result);
    }
}
/**/