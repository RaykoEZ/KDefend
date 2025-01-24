using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class KDefenderStateManager : MonoBehaviour 
{
    [SerializeField] KDefenderGameState m_defaultState = default;
    [SerializeField] GameTimer m_timer = default;
    [SerializeField] UnityEvent<KDefenderGameState> m_onStateUpdate = default;
    [SerializeField] UnityEvent m_onGameOver = default;
    KDefenderGameState m_current;

    public KDefenderGameState Current => m_current;

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
        UpdateState(m_defaultState);
    }
    public void UpdateState(KDefenderGameState newState) 
    {
        m_current = newState;
        m_onStateUpdate?.Invoke(m_current);
    }
    public void OnGameOver() 
    {
        m_onGameOver?.Invoke();
    }
}