using System.Collections.Generic;
using Curry.Events;
using Curry.Game;
using UnityEngine;
using UnityEngine.Events;
public class KDefenderStateManager : MonoBehaviour 
{
    [SerializeField] GameSaveSource m_save = default;

    [SerializeField] ThreatHandler m_threat = default;
    [SerializeField] EnemyManager m_enemy = default;
    [SerializeField] DeliveryManager m_objectives = default;
    [SerializeField] InventoryManager m_inventory = default;
    [SerializeField] Player m_player = default;
    [SerializeField] GameTimer m_timer = default;
    [SerializeField] UnityEvent m_onGameOver = default;
    // As an alternate game mode
    // Will implement with the KeepQuiet saves system
    public void GameSetup(SaveData newState)
    {
        // set player state
        m_player?.Init(newState.KDGameState.PlayerValue);
        m_enemy?.Init(newState.KDGameState.CurrentThreatLevel, newState.KDGameState.HostileStates);
        m_objectives.Init(newState.KDGameState.Completed, newState.KDGameState.Active);
        m_timer.StartTimer();
    }
    // get current states from managers
    public void UpdateSave()
    {
        var newState = new KDefenderGameState
        {
            Timer = m_timer.SecondsElapsed,
            CurrentThreatLevel = m_threat.CurrentThreat,
            PlayerValue = m_player.CurrentStats,
            Inventory = m_inventory.GetState(),
            HostileStates = m_enemy.GetEnemyStates(),
            // Objectives here
            Completed = m_objectives.GetDetailsOf(
                ObjectiveManager<DeliveryDetail>.ObjectiveState.Complete),
            Active = m_objectives.GetDetailsOf(
                ObjectiveManager<DeliveryDetail>.ObjectiveState.Active)
        };
        m_save.Current.KDGameState = newState;
    }
    public void OnGameOver() 
    {
        m_onGameOver?.Invoke();
    }
}