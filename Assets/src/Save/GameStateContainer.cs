using UnityEngine;
[CreateAssetMenu(fileName = "GameState_", menuName = "Game State Preset", order = 0)]
public class GameStateContainer : ScriptableObject 
{
    [SerializeField] SaveData m_state = default;
    public SaveData State => new SaveData(m_state);
    public void SetSaveState(SaveData save) 
    {
        if (save == null) return;
        m_state = save;
    }
}

