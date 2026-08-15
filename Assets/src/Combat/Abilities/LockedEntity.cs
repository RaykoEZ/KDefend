using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// disables the attached entity on startup, activate the entity when player triggers
// a key event.
// e.g. deactivate locks from protected locations
[RequireComponent(typeof(BaseEntity))]
public class LockedEntity : MonoBehaviour
{
    [SerializeField] bool m_lockByDefault = default;
    [SerializeField] List<LockHandler> m_toUnlock = default;
    // when the entity is fully locked/unlocked
    [SerializeField] UnityEvent m_onLock = default;
    [SerializeField] UnityEvent m_onUnlock = default;
    bool m_isLocked = false;
    HashSet<LockHandler> m_unlocked = new HashSet<LockHandler>();
    // Use this for initialization
    void OnEnable()
    {
        foreach (var handler in m_toUnlock) 
        {
            handler.OnLocked += OnLock;
            handler.OnUnlocked += OnUnlock;
        }
    }
    void OnDisable()
    {
        foreach (var handler in m_toUnlock)
        {
            handler.OnLocked -= OnLock;
            handler.OnUnlocked -= OnUnlock;
        }
    }
    void Start() 
    { 
        m_isLocked = m_lockByDefault;
        if (m_isLocked) 
        {
            m_onLock?.Invoke();
        }
        else
        {
            m_onUnlock?.Invoke();
        }
    }
    void OnLock(LockHandler toLock) 
    {
        m_unlocked.Remove(toLock);
        if (!m_isLocked) 
        { 
            m_isLocked = true;
            m_onLock?.Invoke();
        }
    }
    void OnUnlock(LockHandler toUnlock) 
    {
        m_unlocked.Add(toUnlock);
        if (m_unlocked.Count == m_toUnlock.Count) 
        {
            m_onUnlock?.Invoke();
        }
    }
}
