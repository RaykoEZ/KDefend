using UnityEngine;
using UnityEngine.Events;

public class KDefenderStateManager : MonoBehaviour 
{
    [SerializeField] UnityEvent<KDefenderGameState> m_onStateUpdate = default;
    KDefenderGameState m_current;
    public KDefenderGameState TryLoadSaveState()
    {
        KDefenderGameState result = new KDefenderGameState { };
        return result;
    }
    public void SaveToFile() 
    { 
    
    }
    public void UpdateState(KDefenderGameState newState) 
    {
        m_current = newState;
        m_onStateUpdate?.Invoke(m_current);
    }
}
