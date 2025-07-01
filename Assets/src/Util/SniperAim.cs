using UnityEngine;

namespace Curry.Game
{
    public class SniperAim : DirectionArrow 
    {
        private BaseEntity m_target;
        public BaseEntity GetTarget()
        {
            return m_target;
        }
        public void SetTarget(BaseEntity value)
        {
            m_target = value;
        }
        public void ResetTarget()
        {
            m_target = null;
        }
        protected override void Update()
        {
            if (m_target != null) 
            {
                Vector3 dir = m_target.transform.position - m_origin.position;
                RenderLine(m_lineRender, m_origin.position, dir.normalized, m_lengthScale, m_blockingLayers);
            }        
        }
    }
}
