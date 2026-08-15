using UnityEngine;
using UnityEngine.Events;

public class KDefenderDataSource : BaseSaveSource<KDefenderGameState>
{
    [SerializeField] UnityEvent<KDefenderGameState> m_onSave = default;
    public override void UpdateSave(bool saveToFile = false)
    {
        m_onSave?.Invoke(m_currentGameState);
    }
}