using UnityEngine;
using UnityEngine.Events;

public class KDefenderStateManager : MonoBehaviour 
{
    [SerializeField] UnityEvent<KDefenderGameState> m_onStateUpdate = default;
    KDefenderGameState m_current;
    // As an alternate game mode
    // Will implement with the KeepQuiet saves system
    public KDefenderGameState TryLoadSaveState()
    {
        KDefenderGameState result = new KDefenderGameState { };
        return result;
    }
    public void SaveToFile() 
    { 
    
    }
    void Start()
    {
        UpdateState(KDefenderGameState.Default);
    }
    public void UpdateState(KDefenderGameState newState) 
    {
        m_current = newState;
        m_onStateUpdate?.Invoke(m_current);
    }
}