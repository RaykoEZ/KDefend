using UnityEngine;

namespace Curry.Game
{
    public class PoolableBehaviour : MonoBehaviour, IPoolable
    {
        public IObjectPool Origin { get; set; }
        bool m_initialized = false;
        protected virtual void OnEnable()
        {
            if (Origin == null)
            {
                Prepare();
            }
        }
        protected virtual void OnDisable()
        {
            ReturnToPool();
        }
        public virtual void Prepare() 
        {
            if (m_initialized) return;
            m_initialized = true;
        }
        public virtual void ReturnToPool()
        {
            m_initialized = false;
            if (Origin == null) 
            {
                Destroy(gameObject);
            } 
            else 
            {
                Origin.Reclaim(this);
            }
        }
    }
}
