using System;
using System.Collections.Generic;
using Curry.Events;
using Curry.Game;
using UnityEngine;
using UnityEngine.Events;

public class DayCounter
{
    // use this to display short day text with dayOfWeek index
    public static string[] s_dayOfWeekText_Short = new string[]
    {"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"};
    DayOfWeek m_current;
    public DayOfWeek Current { get => m_current; }
    public void SetDay(DayOfWeek current)
    {
        m_current = current;
    }
    public void NextDay()
    {
        int day = (int)m_current % 7;
        m_current = (DayOfWeek)day;
    }
}
// initialises and saves game state
//TODO: Delay Delivery features for now
public class KDefenderStateManager : MonoBehaviour 
{
    [SerializeField] GameSaveSource m_save = default;
    [SerializeField] StaticFlagEventHandler m_staticEvents = default;

    [SerializeField] EnemyManager m_enemy = default;
    [SerializeField] EnemyWaveManager m_wave = default;

    [SerializeField] ShopPoolUpdater m_shopPoolUpdater = default;

    [SerializeField] Player m_player = default;
    [SerializeField] InventoryManager m_inventoryManager = default;
    [SerializeField] GameTimer m_timer = default;
    [SerializeField] UnityEvent m_onGameOver = default;
    [SerializeField] UnityEvent<DayOfWeek> m_onNewDay = default;
    static int s_currentLevel = 0;
    static bool s_isPaused = false;
    DayCounter m_dayOfWeek = new DayCounter();
    public static int CurrentLevel { get => s_currentLevel; }
    public DayOfWeek CurrentDayOfWeek => m_dayOfWeek.Current;
    public static bool IsPaused { get => s_isPaused; }

    // As an alternate game mode
    // Will implement with the KeepQuiet saves system
    // Initialize game state on launch
    public void GameSetup(SaveData newState)
    {
        KDefenderGameState state = newState.KDGameState;
        // set player state
        m_player?.Init(state.PlayerValue);
        m_enemy?.Init(state.HostileStates);
        SetDayOfWeek(state.DayOfWeek);
        m_inventoryManager?.Init(state.HeldItems);
        m_shopPoolUpdater?.InitPool(state.ShopStates);
        m_staticEvents.SetFlags(state.StaticFlags);
        m_timer.SecondsLeft = state.SecondsLeft;
        m_timer.StartTimer();
    }
    // get current states from managers to save latest game state
    public void UpdateSave()
    {
        var newState = new KDefenderGameState
        {
            SecondsLeft = m_timer.SecondsLeft,
            DayOfWeek = m_dayOfWeek.Current,
            StaticFlags = m_staticEvents.CurrentFlags,
            PlayerValue = m_player.CurrentStats,
            HostileStates = m_enemy.GetEnemyStates(),
            HeldItems = m_inventoryManager.GetItemPropeties(),
            ShopStates = m_shopPoolUpdater.CurrentShopPool
        };
        m_save.Current.KDGameState = newState;
    }
    public void OnGameOver() 
    {
        m_onGameOver?.Invoke();
    }
    // for pausing game for menu and item effect
    public void PauseGame() 
    {
        s_isPaused = !s_isPaused;
        Time.timeScale = s_isPaused? 0f : 1f;    
    }
    public void OnNextDay() 
    { 
        m_dayOfWeek?.NextDay();
        UpdateSave();
        m_onNewDay?.Invoke(CurrentDayOfWeek);
    }
    public void SetDayOfWeek(DayOfWeek newDay) 
    {
        m_dayOfWeek?.SetDay(newDay);
        UpdateSave();
        m_onNewDay?.Invoke(CurrentDayOfWeek);
    }
}