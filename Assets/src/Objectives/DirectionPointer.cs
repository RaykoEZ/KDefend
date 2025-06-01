using UnityEngine;
using UnityEngine.UI;
using System;

// points to an object of interest
public class DirectionPointer : MonoBehaviour 
{
    [SerializeField] Bounds m_displayOffsets = default;
    [SerializeField] Transform m_player = default;
    [SerializeField] Image m_arrow = default;
    Transform m_currentTraget;
    bool m_isPointing = false;

    public bool IsPointing { get => m_isPointing; }

    void FixedUpdate()
    {
        if (m_isPointing) 
        {
            UpdateDirection();
        }
    }
    public void PointToward(Transform newTarget) 
    {
        m_currentTraget = newTarget;
        m_isPointing = true;
        m_arrow.enabled = true;
    }
    public void StopPointing() 
    {
        m_isPointing = false;
        m_arrow.enabled = false;
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
    // Detect destination nearby
    void OnTriggerEnter2D(Collider2D collision)
    {
        m_arrow.enabled = false;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        m_arrow.enabled = true;
    }
}
