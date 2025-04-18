using System.Collections.Generic;
using Curry.Events;
using Curry.Game;
using UnityEngine;
using UnityEngine.Events;
public class KDefenderStateManager : MonoBehaviour 
{
    [SerializeField] KDefenderGameState m_defaultState = default;
    [SerializeField] KDefenderDataSource m_dataSource = default;

    [SerializeField] EnemyManager m_enemy = default;
    [SerializeField] DeliveryManager m_objectives = default;
    [SerializeField] InventoryManager m_inventory = default;
    [SerializeField] Player m_player = default;
    [SerializeField] GameTimer m_timer = default;
    [SerializeField] UnityEvent<KDefenderGameState> m_onStateUpdate = default;
    [SerializeField] UnityEvent m_onGameOver = default;
    int m_currentLevel = 0;
    int m_killCount = 0;
    // As an alternate game mode
    // Will implement with the KeepQuiet saves system
    public void TryLoadSaveState(KDefenderGameState newState)
    {
        m_currentLevel = newState.CurrentThreatLevel;
        m_killCount = newState.EnemiesKilled;
        // set player state
        m_currentLevel = newState.CurrentThreatLevel;
        m_player?.Init(newState.PlayerValue);
        m_enemy?.Init(newState.HostileStates);
        m_objectives.Init(newState.Completed, newState.Active);
        m_onStateUpdate?.Invoke(newState);
    }
    // get current states from managers
    public void SyncSave() 
    {
        var newState = new KDefenderGameState
        {
            CurrentThreatLevel = m_currentLevel,
            EnemiesKilled = m_killCount,
            PlayerValue = m_player.CurrentStats,
            Inventory = m_inventory.GetState(),
            HostileStates = m_enemy.GetEnemyStates(),
            // Objectives here
            Completed = m_objectives.GetDetailsOf(
                ObjectiveManager<DeliveryDetail>.ObjectiveState.Complete),
            Active = m_objectives.GetDetailsOf(
                ObjectiveManager<DeliveryDetail>.ObjectiveState.Active)
        };
        m_dataSource.Init(newState);
    }
    void Start()
    {
        m_dataSource?.Init(m_defaultState);
        TryLoadSaveState(m_defaultState);
        m_timer.StartTimer();
    }
    public void OnGameOver() 
    {
        m_onGameOver?.Invoke();
    }
}