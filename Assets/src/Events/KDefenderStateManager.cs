using System.Collections.Generic;
using Curry.Game;
using UnityEngine;
using UnityEngine.Events;
public class KDefenderStateManager : MonoBehaviour 
{
    [SerializeField] GameSaveSource m_save = default;
    [SerializeField] StaticFlagEventHandler m_staticEvents = default;
    [SerializeField] ThreatHandler m_threat = default;
    [SerializeField] EnemyManager m_enemy = default;

    [SerializeField] DeliveryDropTable m_deliveryList = default;
    [SerializeField] DeliveryManager m_objectives = default;

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
        
        List<DeliveryDetail> completed = m_deliveryList.Find(newState.KDGameState.CompletedDeliveries);
        List<DeliveryDetail> active = m_deliveryList.Find(newState.KDGameState.ActiveDeliveries);
        m_objectives.Init(completed, active);
        m_staticEvents.SetFlags(newState.KDGameState.StaticFlags);
        m_timer.StartTimer();
    }
    // get current states from managers
    public void UpdateSave()
    {
        List<string> completed = DeliveryDetail.GetTitleList(
            m_objectives.GetDetailsOf(
                ObjectiveManager<DeliveryDetail>.ObjectiveState.Complete));
        List<string> active = DeliveryDetail.GetTitleList(
            m_objectives.GetDetailsOf(
                ObjectiveManager<DeliveryDetail>.ObjectiveState.Active));
        var newState = new KDefenderGameState
        {
            Timer = m_timer.SecondsElapsed,
            StaticFlags = m_staticEvents.CurrentFlags,
            CurrentThreatLevel = m_threat.CurrentThreat,     
            PlayerValue = m_player.CurrentStats,
            HostileStates = m_enemy.GetEnemyStates(),
            // Objectives here
            CompletedDeliveries = completed,
            ActiveDeliveries = active
        };
        m_save.Current.KDGameState = newState;
    }
    public void OnGameOver() 
    {
        m_onGameOver?.Invoke();
    }
}