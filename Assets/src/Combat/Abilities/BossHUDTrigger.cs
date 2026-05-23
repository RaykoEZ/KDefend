using UnityEngine;

public class BossHUDTrigger : MonoBehaviour 
{
    [SerializeField] DisplayInfo m_displayInfo = default;
    [SerializeField] BossHUDHandler m_display = default;
    public void Show() 
    {
        m_display?.DisplayBoss(m_displayInfo);
    }
}