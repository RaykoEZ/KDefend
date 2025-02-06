using UnityEngine;
using UnityEngine.Events;

public class KDefenderDataSource : BaseSaveSource<KDefenderGameState>
{
    [SerializeField] KDefenderGameState m_defaultState = default;
    [SerializeField] UnityEvent<KDefenderGameState> m_onSave = default;
    void Start()
    {
        Init(m_defaultState);
        UpdateSave();
    }

    public override void UpdateSave(bool saveToFile = false)
    {
        m_onSave?.Invoke(m_currentGameState);
    }
}