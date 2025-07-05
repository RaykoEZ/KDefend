using UnityEngine;
using Curry.Game;

public class SniperLaser : DirectionArrow 
{    
    // point line towards a target position
    public RaycastHit2D PointTowards(Vector3 target) 
    {
        RaycastHit2D visual;
        Vector3 dir = target - m_origin.position;
        visual = RenderLine(m_lineRender, m_origin.position, dir.normalized, m_lengthScale, m_blockingLayers);
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
