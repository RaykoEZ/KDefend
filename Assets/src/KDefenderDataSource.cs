using UnityEngine;
using UnityEngine.Events;

public class KDefenderDataSource : BaseSaveSource<KDefenderGameState>
{
    [SerializeField] KDefenderStateManager m_gameState = default;
    [SerializeField] UnityEvent<KDefenderGameState> m_onSave = default;
    public override void UpdateSave(bool saveToFile = false)
    {
        m_gameState?.UpdateSave();
        m_onSave?.Invoke(m_currentGameState);
    }
}