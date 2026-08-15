using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
// Script for persistent game state loading and saving
// Loads persistent game state into game state manager in scene
// Saves updated states coming from game state in scene
public class GameStateFileHandler : MonoBehaviour
{
    [SerializeField] GameStateContainer m_testState = default;
    // State to load upon first load
    [SerializeField] GameStateContainer m_defaultState = default;
    // current save data in SO when changing scenes 
    [SerializeField] GameStateContainer m_currentCache = default;
    [SerializeField] UnityEvent<SaveData> m_InitSceneCallbacks = default;
    SaveData m_current;
    static bool s_saveInProgress = false;
    static string s_gamestatePath = "saves/gamestate.json";
    public SaveData Current => new SaveData(m_current);
    void Start()
    {
        // for editor testing,
        // Setup game states on game launch
        int sceneIdx = SceneManager.GetActiveScene().buildIndex;
        if (sceneIdx == 0)
        {
            // Start game launch, we load save from file
            LoadFromFile();
            m_InitSceneCallbacks?.Invoke(Current);
        }
        else if (Application.isEditor) 
        {
            m_currentCache.SetSaveState(m_testState.State);
            // If we are in other scenes, load cache from previous scene
            TryLoadFromCache();
        }
        else 
        {
            // If we are in other scenes, load cache from previous scene
            TryLoadFromCache();
        }
    }
    // load from SaveDataSource
    void TryLoadFromCache()
    {
        // fallback to loading from file if
        // cached state is null
        SaveData save = m_currentCache?.State;
        if (save == null) 
        {
            LoadFromFile();
        }
        else 
        {
            m_current = save;
        }
        m_InitSceneCallbacks?.Invoke(Current);
    }
    // For getting the latest save data, then saving the game
    public void UpdateSave(SaveData update, bool saveToFile = false)
    {
        if (update == null) return;
        m_current = new SaveData(update);
        m_currentCache.SetSaveState(m_current);
        if (saveToFile) 
        {
            SaveToFile(m_current);
        }
    }
    // Set a new game with persistent states kept
    public void SetupNewGame()
    {
        // copy persisting save from current
        SaveData.PersistentSave persist;
        if (m_current == null) 
        {
            persist = new SaveData.PersistentSave();
            // assign new ID
            persist.PlayerID = GameUtil.RandomNumberID();
        }
        else 
        {
            persist = new SaveData.PersistentSave(m_current.Persistent);
        }
        // reset game state to new game
        m_current = new SaveData(m_defaultState.State);
        // set persistent save states
        m_current.Persistent = persist;
        m_currentCache.SetSaveState(m_current);
        SaveToFile(m_current);
    }
    // Reset all save data to new game state,
    // inc. persistent states, current cache copies & previous save file
    public void ResetAllData() 
    {
        m_current = new SaveData(m_defaultState.State);
        m_currentCache.SetSaveState(m_current);
        SaveToFile(m_current);
    }
#region File Operations
    // Read Meta File states and Locations to update game state
    protected void LoadFromFile() 
    {
        // no loading when saving
        if (s_saveInProgress) return;
        if (!File.Exists($"{FileUtil.s_gamestateSavePath}/{s_gamestatePath}")) 
        {
            SetupNewGame();
            return;
        }
        using (StreamReader r = new StreamReader($"{FileUtil.s_gamestateSavePath}/{s_gamestatePath}"))
        {
            string json = r.ReadToEnd();
            SaveData loaded = JsonConvert.DeserializeObject<SaveData>(json);
            m_current = loaded;
        }
        // set cache for future scenes
        m_currentCache.SetSaveState(m_current);
    }
    protected void SaveToFile(SaveData save) 
    {
        // if we are saving after finish an ending, increment new gamw counter
        SaveData newSave = new SaveData(save);
        string json = JsonConvert.SerializeObject(newSave, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        FileUtil.RawTextTo(FileUtil.s_gamestateSavePath, "saves","gamestate.json", new string[] { json });
    }
#endregion
}