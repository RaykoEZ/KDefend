using Curry.Game;
using UnityEngine;
using UnityEngine.InputSystem;
public class Laser : DirectionArrow 
{
    // point line towards a target position
    public RaycastHit2D PointTowards(Vector3 target) 
    {
        RaycastHit2D visual;
        m_aimDirection = (target - m_origin.position).normalized;
        visual = RenderLine(m_lineRender, m_origin.position, target, m_blockingLayers);
        return visual;
    }
    // point line towards a target position's direction, with scaling length
    public RaycastHit2D PointTowardsDirection(Vector3 dirNormalize, bool passThroughTarget = false)
    {
        RaycastHit2D visual;
        m_aimDirection = dirNormalize;
        visual = RenderLine(m_lineRender, m_origin.position, dirNormalize, m_lengthScale, m_blockingLayers, passThroughTarget);
        return visual;
    }
    // point line towards a target position's direction, with scaling length and origin position
    public RaycastHit2D PointTowardsDirection(Vector3 origin, Vector3 dirNormalize, bool passThroughTarget = false)
    {
        RaycastHit2D visual;
        m_aimDirection = dirNormalize;
        visual = RenderLine(m_lineRender, origin, dirNormalize, m_lengthScale, m_blockingLayers, passThroughTarget);
        return visual;
    }
    public void Clear()
    {
        m_lineRender.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
    }
    protected override void Update()
    {
    }
}

