using UnityEngine;
using PixelCrushers.DialogueSystem;
// You can use these functions as models and then replace them with your own.

// Triggers KDefender Game Event to spawn enemies, and include flag for this spawn event, for Dialogue System toolkit
public class DialogueLua_SpawnEvent : MonoBehaviour
{
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    public bool unregisterOnDisable = false;
    [Tooltip("New event flag for this spawn event")]
    [SerializeField] KD_StaticEventFlags m_raiseEventFlag = default;
    [Tooltip("SO Asset file need to be unique for each of this spawn wave event")]
    [SerializeField] SpawnWave m_toSpawn = default;
    string m_spawnFunctionID => $"{nameof(Spawn)}_{m_spawnName}";
    string m_spawnName = "Default";
    void OnEnable()
    {
        m_spawnName = m_toSpawn.name;
        // Make spawn function ID bound to each unique spawn wave 
        Lua.RegisterFunction(m_spawnFunctionID, this, SymbolExtensions.GetMethodInfo(() => Spawn()));
    }
    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction(m_spawnFunctionID);
        }
    }
    public void Spawn()
    {
        KDEventUtil.SpawnEnemies(this, m_toSpawn, m_raiseEventFlag);
    }  
}
/**/