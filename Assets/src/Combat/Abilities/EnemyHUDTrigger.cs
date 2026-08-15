using UnityEngine;

public class EnemyHUDTrigger : MonoBehaviour 
{
    [SerializeField] bool m_triggerOnStartup = default;
    [SerializeField] DisplayInfo m_displayInfo = default;
    [SerializeField] EnemyHUDHandler m_display = default;
    void Start()
    {
        if (m_triggerOnStartup) 
        {
            Show();
        }
    }
    public void Show() 
    {
        m_display?.DisplayBoss(m_displayInfo);
    }
}