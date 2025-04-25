using Curry.Events;
using System.Collections.Generic;
using UnityEngine;
// Listens to a collection of game events to execute set scene events
public class GameEventHandler : MonoBehaviour 
{
    [SerializeField] List<CurryGameEventListener> m_toListen = default;
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
