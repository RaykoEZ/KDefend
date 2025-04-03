using Curry.Events;
using Curry.Game;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

public class KDefenderStateManager : MonoBehaviour 
{
    [SerializeField] KDefenderGameState m_defaultState = default;
    [SerializeField] KDefenderDataSource m_dataSource = default;
    [SerializeField] ObjectiveManager m_objectives = default;
    [SerializeField] Player m_player = default;
    [SerializeField] GameTimer m_timer = default;
    [SerializeField] UnityEvent<KDefenderGameState> m_onStateUpdate = default;
    [SerializeField] UnityEvent m_onGameOver = default;
    // As an alternate game mode
    // Will implement with the KeepQuiet saves system
    public KDefenderGameState TryLoadSaveState()
    {
        KDefenderGameState result = new KDefenderGameState { };
        return result;
    }
    public void UpdateSave(KDefenderGameState newState) 
    {
        // set player state
        m_player?.Init(newState.PlayerValue);
        m_onStateUpdate?.Invoke(newState);
    }
    void Start()
    {
        m_dataSource?.Init(m_defaultState);
        UpdateSave(m_defaultState);
        m_timer.StartTimer();
    }
    public void ObjectiveComplete(IObjective obj) 
    { 
    
    }
    public void OnGameOver() 
    {
        m_onGameOver?.Invoke();
    }
}