using UnityEngine;
using System.Collections.Generic;
using Curry.Util;
// Units in phalanx hold their position, moves as center commander moves
public class Phalanx : Formation
{
    [SerializeField] Transform m_center = default;
    List<Vector2> m_positionOffsets = new List<Vector2>();
    void Start() 
    {
        Vector2 offset;
        foreach (var member in m_members)
        {
            offset = member.position - m_center.position;
            m_positionOffsets.Add(offset);
        }
    }
    public override bool TryGetFormationPosition(BaseEntity chaseTarget, out Vector3 warpPosition)
    {
        int i = m_members.FindIndex((x) => x == chaseTarget.transform);
        if (i > 0 && i < m_members.Count) 
        {
            // Move target location as center moves
            warpPosition = VectorExtension.ToVec2(m_center.position) + m_positionOffsets[i];
        }
        else
        {
            warpPosition = chaseTarget.transform.position;
        }
        return i > 0 && i < m_members.Count;
    }
}
