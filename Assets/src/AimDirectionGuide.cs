using UnityEngine;
using UnityEngine.InputSystem;
namespace Curry.Explore
{
    public delegate void OnGuideFinish();
    // Display guide line when choosing targets with card effects
    public class AimDirectionGuide : MonoBehaviour
    {
        [SerializeField] LineRenderer m_line = default;
        Transform m_origin;
        void Start()
        {
            BeginLine(transform);
        }
        void Update()
        {
            UpdateLine();
        }
        public void BeginLine(Transform origin)
        {
            m_origin = origin;
            m_line.positionCount = 2;
            Vector2 pos = new Vector2(m_origin.position.x, m_origin.position.y);
            // Setting each point to prevent previous dirty points appearing before updating
            m_line.SetPosition(1, pos);
            m_line.SetPosition(0, pos);
        }
        public void UpdateLine()
        {
            Vector2 world = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 pos = new Vector2(m_origin.position.x, m_origin.position.y);
            // Show small arrow near the player rather than a full line
            Vector2 dir = (world - pos).normalized;
            m_line.SetPosition(0, pos + 0.75f * dir);
            m_line.SetPosition(1, pos + 1.25f * dir);
        }
    }

}