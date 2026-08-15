using UnityEngine;
using UnityEngine.Events;
public delegate void OnLockUpdate(LockHandler toUpdate);
// To lock away the LockdownEntity object
// When unlocked, feedback to lock-owner LockdownEntity 
public class LockHandler : MonoBehaviour 
{
    // For the lock's local lock/unlock events
    [SerializeField] UnityEvent m_onLock = default;
    [SerializeField] UnityEvent m_onUnlock = default;
    public event OnLockUpdate OnLocked;
    public event OnLockUpdate OnUnlocked;
    public void Lock() 
    {
        m_onLock?.Invoke();
        OnLocked?.Invoke(this);
    }
    public void Unlock() 
    {
        m_onUnlock?.Invoke();
        OnUnlocked?.Invoke(this);
    }
}