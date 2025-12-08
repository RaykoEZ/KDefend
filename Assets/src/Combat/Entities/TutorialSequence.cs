using UnityEngine;
using UnityEngine.Events;
public class TutorialSequence : MonoBehaviour
{
    [SerializeField] UnityEvent m_movement = default;
    [SerializeField] UnityEvent m_attack = default;
    [SerializeField] UnityEvent m_dash = default;
    [SerializeField] UnityEvent m_allCleared = default;
    bool m_moveCleared = false;
    bool m_attackCleared = false;
    bool m_dashCleared = false;
    public bool AllCleared => m_moveCleared && m_attackCleared && m_dashCleared;

    public void MovingGuideCleared() 
    {
        m_movement?.Invoke();
        m_moveCleared = true;
        CheckAllCleared();
    }
    public void AttackGuideCleared() 
    {
        m_attack?.Invoke();
        m_attackCleared = true;
        CheckAllCleared();
    }
    public void DashGuideCleared() 
    {
        m_dash?.Invoke();
        m_dashCleared = true;
        CheckAllCleared();
    }
    public void CheckAllCleared() 
    {
        if (AllCleared) 
        { 
            m_allCleared?.Invoke();
        }
    }
}