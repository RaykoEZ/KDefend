using UnityEngine;
// points to an object of interest
public class DirectionPointer : MonoBehaviour 
{
    [SerializeField] Bounds m_displayOffsets = default;
    [SerializeField] Transform m_defaultTarget = default;
    [SerializeField] Transform m_player = default;
    Transform m_currentTraget;
    void Start()
    {
        UpdatePointingTarget(m_defaultTarget);
    }
    void FixedUpdate()
    {
        UpdateDirection();
    }
    public void UpdatePointingTarget(Transform newTarget) 
    {
        m_currentTraget = newTarget;
    }
    void UpdateDirection() 
    {
        if (m_currentTraget == null) return;
        Vector3 origin = m_player.position;
        Vector3 lowLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 upRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        float iconX = Mathf.Clamp(m_currentTraget.position.x, lowLeft.x + m_displayOffsets.extents.x, upRight.x - m_displayOffsets.extents.x);
        float iconY = Mathf.Clamp(m_currentTraget.position.y, lowLeft.y + m_displayOffsets.extents.y, upRight.y - m_displayOffsets.extents.y);
        Vector3 iconPos = new Vector3(iconX, iconY);
        transform.position = iconPos;
        Vector3 dir = (m_currentTraget.position - origin).normalized;
        GameUtil.AimTowards2D(transform, dir);
    }
}
