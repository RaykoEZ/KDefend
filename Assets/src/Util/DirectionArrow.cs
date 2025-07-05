using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Curry.Game
{
    // for player aim laser
    [RequireComponent(typeof(LineRenderer))]
    public class DirectionArrow : MonoBehaviour
    {
        [SerializeField] protected LayerMask m_blockingLayers = default;
        [SerializeField] protected Transform m_origin = default;
        [SerializeField] protected LineRenderer m_lineRender = default;
        [SerializeField] protected float m_lengthScale = default;
        Vector2 m_mousePos = Vector2.zero;
        // Update is called once per frame
        protected virtual void Update()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            if (m_mousePos != mousePos) 
            {
                m_mousePos = mousePos;
                Vector2 origin = m_origin.position;
                Vector2 dir = m_mousePos - origin;
                RenderLine(m_lineRender, origin, dir.normalized, m_lengthScale, m_blockingLayers);
            }
        }
        internal static RaycastHit2D RenderLine(LineRenderer renderer, Vector3 origin, Vector3 directionNormalized, float distance, LayerMask collideWith) 
        {
            Vector3[] line = { Vector3.zero, Vector3.zero };
            line[0] = origin;
            Vector2 dest = origin + (directionNormalized * distance);
            // collision test
            RaycastHit2D hit = Physics2D.Linecast(origin, dest, collideWith);
            // stop at destination or collision position
            line[1] = hit ? hit.point : dest;
            renderer?.SetPositions(line);
            return hit;
        }
    }
}
