using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DialogueLua_OnRetreat : MonoBehaviour
{
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    public bool unregisterOnDisable = false;
    [SerializeField] DialogueDatabase m_database = default;
    string m_spawnFunctionID => nameof(OnRetreat);

    void OnEnable()
    {
        // Make spawn function ID bound to each unique spawn wave 
        Lua.RegisterFunction(m_spawnFunctionID, this, SymbolExtensions.GetMethodInfo(() => OnRetreat()));
    }
    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction(m_spawnFunctionID);
        }
    }
    public void OnRetreat()
    {
        int num = DialogueLua.GetVariable("NumRetreat").AsInt;
        DialogueLua.SetVariable("NumRetreat", num++);
    }
}
/**/