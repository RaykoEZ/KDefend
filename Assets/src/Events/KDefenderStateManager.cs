using System.Collections.Generic;
using Curry.Game;
using Curry.Events;
using UnityEngine;
using UnityEngine.Events;
// initialises and saves game state
public class KDefenderStateManager : MonoBehaviour 
{
    [SerializeField] GameSaveSource m_save = default;
    [SerializeField] StaticFlagEventHandler m_staticEvents = default;

    [SerializeField] EnemyManager m_enemy = default;
    [SerializeField] EnemyWaveManager m_wave = default;
    // Delay this feature for now
    //[SerializeField] DeliveryDropTable m_deliveryList = default;
    //[SerializeField] DeliveryManager m_objectives = default;
    [SerializeField] ShopPoolUpdater m_shopPoolUpdater = default;

    [SerializeField] Player m_player = default;
    [SerializeField] InventoryManager m_inventoryManager = default;
    [SerializeField] GameTimer m_timer = default;
    [SerializeField] UnityEvent m_onGameOver = default;
    static int s_currentLevel = 0;
    public static int CurrentLevel { get => s_currentLevel; }

    // As an alternate game mode
    // Will implement with the KeepQuiet saves system
    // Initialize game state on launch
    public void GameSetup(SaveData newState)
    {
        KDefenderGameState state = newState.KDGameState;
        // set player state
        m_player?.Init(state.PlayerValue);
        m_enemy?.Init(state.HostileStates);
        m_inventoryManager?.Init(state.HeldItems);
        m_shopPoolUpdater?.InitPool(state.ShopStates);

        //List<DeliveryDetail> active = m_deliveryList.Find(state.ActiveDeliveries);
        //m_objectives.Init(active);
        m_staticEvents.SetFlags(state.StaticFlags);
        m_timer.StartTimer();
    }
    // get current states from managers to save latest game state
    public void UpdateSave()
    {
        /*List<string> completed = DeliveryDetail.GetTitleList(
            m_objectives.GetDetailsOf(
                ObjectiveManager<DeliveryDetail>.ObjectiveState.Complete));
        List<string> active = DeliveryDetail.GetTitleList(
            m_objectives.GetDetailsOf(
                ObjectiveManager<DeliveryDetail>.ObjectiveState.Active));*/
        var newState = new KDefenderGameState
        {
            Timer = m_timer.SecondsElapsed,
            StaticFlags = m_staticEvents.CurrentFlags,
            PlayerValue = m_player.CurrentStats,
            HostileStates = m_enemy.GetEnemyStates(),
            HeldItems = m_inventoryManager.GetItemPropeties(),
            //ActiveDeliveries = active,
            ShopStates = m_shopPoolUpdater.CurrentShopPool
        };
        m_save.Current.KDGameState = newState;
    }
    public void OnGameOver() 
    {
        m_onGameOver?.Invoke();
    }
    // trigger intel recovery protocol on enemy side
    public void OnIntelPickup(EventInfo info)
    {
        
        if (info == null || info.Payload == null) return;
        bool spawn = info.Payload.TryGetValue("wave", out object t1) &&
            t1 is SpawnWave;
        if (spawn)
        {
            // spawn elite/boss wave
            m_wave?.SpawnWave(t1 as SpawnWave);
            // Spawn intel decrypter
        }
    }
}