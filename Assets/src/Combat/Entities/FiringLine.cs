using Curry.Explore;
using Curry.Game;
using System.Collections.Generic;
using UnityEngine;

public class FiringLine : Formation
{
    public override bool TryGetFormationPosition(BaseEntity chaseTarget, out Vector3 warpPosition)
    {
        int i = m_members.FindIndex((x) => x == chaseTarget.transform);
        if (i > 0 && i < m_members.Count)
        {
            // Move target location as center moves
            warpPosition = m_members[i].position;
        }
        else
        {
            warpPosition = chaseTarget.transform.position;
        }
        return i > 0 && i < m_members.Count;
    }
}