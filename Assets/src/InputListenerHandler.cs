using UnityEngine;
using System.Collections.Generic;

public class InputListenerHandler : MonoBehaviour 
{
    [SerializeField] bool m_enableOnInit = default;
    [SerializeField] List<TemporaryInputAction> m_listeners = default;
    void OnEnable() 
    {
        if (m_enableOnInit) 
        {
            EnableAll();
        }
    }
    void OnDisable()
    {
        DisableAll();
    }
    public void EnableAll() 
    {
        foreach (var item in m_listeners)
        {
            item?.Enable();
        }
    }
    public void DisableAll()
    {
        foreach (var item in m_listeners)
        {
            item?.Disable();
        }
    }
}