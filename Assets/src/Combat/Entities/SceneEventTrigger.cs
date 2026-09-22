using UnityEngine;
using UnityEngine.Events;

public class SceneEventTrigger : MonoBehaviour 
{
    [SerializeField] UnityEvent m_toTrigger = default;
    public void Trigger() 
    {
        m_toTrigger?.Invoke();
    }
}