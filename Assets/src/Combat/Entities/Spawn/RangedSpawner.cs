using Curry.Game;
using UnityEngine;
using UnityEngine.UIElements;
// A script tp spawn an Entity within specified collider bound
public class RangedSpawner : MonoBehaviour 
{
    [SerializeField] BaseInstanceManager m_instanceManager = default;
    [SerializeField] Collider2D m_spawnRange = default;
    public T Spawn<T>(T spawnRef, Transform parent, bool clearZ = true) where T : MonoBehaviour
    {
        Vector3 spawnPos = GameUtil.RandomPositionInBounds(m_spawnRange.bounds);
        // get pooled instance for new spawn, or make new pooled items
        PoolableBehaviour newBehaviour = m_instanceManager.
        GetInstanceFromAsset(spawnRef.gameObject, parent);
        newBehaviour.transform.position = spawnPos;
        T ret = newBehaviour.GetComponent<T>();
        // To stop instance from inheriting z position to stop z fighting 
        if (clearZ && ret != null) 
        {
            Vector3 pos = ret.transform.localPosition;
            pos.z = 0f;
            ret.transform.localPosition = pos;
        }
        return ret;
    }
}
