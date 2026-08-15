using Curry.Events;
using System.Collections.Generic;
using UnityEngine;
// Listens to a collection of game events to execute set scene events
public class GameEventHandler : MonoBehaviour 
{
    [SerializeField] protected List<CurryGameEventListener> m_toListen = default;
    void OnEnable()
    {
        foreach (var item in m_toListen)
        {
            item?.Init();
        }
    }
    void OnDisable()
    {
        foreach (var item in m_toListen)
        {
            item?.Shutdown();
        }
    }
}
public class NpcEventHandler : GameEventHandler 
{
    [SerializeField] List<KDefenderNpcHandler> m_npcHandles = default;
    public virtual void Init(SaveData saveData) 
    {
        foreach (var item in m_npcHandles) 
        {
            item?.Init(saveData);
        }
    }
    public void HandleEvent(EventInfo eventInfo) 
    {
        if (eventInfo == null) return;
        if (eventInfo is KDEventInfo npcEvent) 
        {
            foreach (var item in m_npcHandles) 
            { 
                item?.HandleEvent(npcEvent);
            }
        }
    }
}
