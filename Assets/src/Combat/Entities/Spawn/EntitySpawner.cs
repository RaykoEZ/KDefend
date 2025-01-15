using System;
using UnityEngine;

public abstract class EntitySpawner<T> : MonoBehaviour where T : BaseEntity
{
    public abstract void Spawn(T spawnRef,
        int numToSpawn = 1, float spawnDelayInterval = 0.1f,
        Action<T> onSpawnAction = null);
}
