using Curry.Events;
using Curry.Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;
public class EnemyManager : MonoBehaviour 
{
    [SerializeField] Transform m_spawnParent = default;
    [SerializeField] Enemy m_scoutRef = default;
    [SerializeField] Enemy m_agentRef = default;
    List<BaseEntity> m_activeEnemies = default;

    public IReadOnlyList<BaseEntity> ActiveEnemies { get => m_activeEnemies;}
    public void SpawnEnemies(List<EnemyState> states) 
    {
        Enemy spawnRef;
        foreach (var item in states)
        {
            spawnRef = GetSpawnRef(item);
            var instance = GameUtil.SpawnObject(spawnRef,
                item.State.Position, m_spawnParent);
            m_activeEnemies.Add(instance);
        }
    }
    protected Enemy GetSpawnRef(EnemyState state) 
    {
        Enemy ret = null;
        switch (state.Type)
        {
            case EnemyType.Scout:
                ret = m_scoutRef;
                break;
            case EnemyType.Agent:
                ret = m_agentRef;
                break;
            case EnemyType.Command:
                break;
            default:
                ret = m_scoutRef;
                break;
        }
        return ret;
    }
    public void OnEnemySpawned(List<BaseEntity> spawned) 
    {
        if (spawned == null || spawned.Count == 0) return;
        m_activeEnemies.AddRange(spawned);
    }
}
public class KDefenderStateManager : MonoBehaviour 
{
    [SerializeField] KDefenderGameState m_defaultState = default;
    [SerializeField] KDefenderDataSource m_dataSource = default;

    [SerializeField] EnemyManager m_enemy = default;
    [SerializeField] ObjectiveManager m_objectives = default;
    [SerializeField] InventoryManager m_inventory = default;
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