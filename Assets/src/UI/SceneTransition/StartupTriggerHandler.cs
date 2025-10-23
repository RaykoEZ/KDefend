using UnityEngine;
using UnityEngine.Events;

public class StartupTriggerHandler : MonoBehaviour 
{
    [SerializeField] UnityEvent m_onGameLaunch = default;
    void Start()
    {
        m_onGameLaunch?.Invoke();
    }
}
