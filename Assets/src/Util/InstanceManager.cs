using Curry.Game;
using System;
using UnityEngine;

namespace Curry.Game
{
    public abstract class InstanceManager<T> where T : MonoBehaviour, IPoolable
    {
        protected abstract PoolCollection<T> Pool { get; }
        ObjectPool<T> m_currentPool = default;
        public Transform DefaultParent => Pool.DefaultParent;
        public virtual void PrepareNewInstance(GameObject instance, Transform parent = null)
        {
            string instanceId = instance.name;
            if (Pool.ContainsPool(instanceId))
            {
                m_currentPool = Pool.GetPool(instanceId);
            }
            else if (instance.TryGetComponent(out T reference))
            {
                m_currentPool = Pool.AddPool(instanceId, reference, parent);
            }
        }

        public virtual T GetInstanceFromAsset(GameObject asset, Transform parent = null)
        {
            PrepareNewInstance(asset, parent);
            return m_currentPool?.GetItemFromPool();
        }
    }
}

[Serializable]
public class BasePoolCollection : PoolCollection<PoolableBehaviour>
{
}
[Serializable]
public class BaseInstanceManager : InstanceManager<PoolableBehaviour>
{
    [SerializeField] protected BasePoolCollection m_pool = default;
    protected override PoolCollection<PoolableBehaviour> Pool { get { return m_pool; } }
}