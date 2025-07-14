using UnityEngine;
using Curry.Game;
public class Laser : DirectionArrow 
{
    // point line towards a target position
    public RaycastHit2D PointTowards(Vector3 target) 
    {
        RaycastHit2D visual;

        visual = RenderLine(m_lineRender, m_origin.position, target, m_blockingLayers);
        return visual;
    }
    // point line towards a target position's direction, with scaling length
    public RaycastHit2D PointTowardsDirection(Vector3 dirNormalize)
    {
        RaycastHit2D visual;
        visual = RenderLine(m_lineRender, m_origin.position, dirNormalize, m_lengthScale, m_blockingLayers);
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

