using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// spawns enemy in a box-patterned outline
// handles onDefeat and onSpawn for spawned instances
public class BoxSpawner : MonoBehaviour
{
    [SerializeField] int m_numToSpawnVerticalSide = default;
    [SerializeField] int m_numToSpawnHorizontalSide = default;
    // Spawner for each corner, left to right , bottom to top
    [SerializeField] RangedSpawner m_spawnerTop = default;
    [SerializeField] RangedSpawner m_spawnerBottom = default;
    [SerializeField] RangedSpawner m_spawnerLeft = default;
    [SerializeField] RangedSpawner m_spawnerRight = default;
    // spawn premise for arena
    [SerializeField] BoxCollider2D m_arenaBounds = default;
    Vector3 m_defaultTop;
    Vector3 m_defaultBottom;
    Vector3 m_defaultLeft;
    Vector3 m_defaultRight;
    List<Enemy> m_spawnRefs = new List<Enemy>();
    public IReadOnlyList<Enemy> SpawnRefs { get => m_spawnRefs; }

    void OnEnable()
    {
        // set default spawner positions
        m_defaultTop = m_spawnerTop.transform.position;
        m_defaultBottom = m_spawnerBottom.transform.position;
        m_defaultLeft = m_spawnerLeft.transform.position;
        m_defaultRight = m_spawnerRight.transform.position;
    }
    // despawns all active instances
    public void Clear() 
    {
        // redirect collection and clear old list for despawning enemies in list
        List<Enemy> enemies = new(m_spawnRefs);
        m_spawnRefs?.Clear();
        foreach (var item in enemies)
        {
            // kill it
            item?.TakeDamage(99999);
        }
    }
    void OnDefeat(Enemy defeated) 
    {
        defeated.OnDefeated -= OnDefeat;
        m_spawnRefs.Remove(defeated);
    }
    public void Spawn(Enemy spawnRef, Transform parent, float spawnDelayInterval = 0.1F, Action<Enemy> onSpawnAction = null)
    {
        // start spawning on all sides of the arena
        StartCoroutine(Spawn_Horizontal(spawnRef, parent, spawnDelayInterval, onSpawnAction));
        StartCoroutine(Spawn_Vertical(spawnRef, parent, spawnDelayInterval, onSpawnAction));
    }
    // spawn for top & bottom side
    IEnumerator Spawn_Horizontal(Enemy spawnRef, Transform parent, float spawnDelayInterval = 3f, Action<Enemy> onSpawnAction = null)
    {
        int numSpawned = 0;
        float xDist = m_arenaBounds.bounds.max.x - m_arenaBounds.bounds.min.x;
        float distInterval = xDist / m_numToSpawnHorizontalSide;
        float moveIntervalPerSpawn = Mathf.Max(1f, distInterval);
        Vector3 move = new Vector3(moveIntervalPerSpawn, 0f, 0f);
        while (numSpawned < m_numToSpawnHorizontalSide)
        {
            yield return new WaitForSeconds(spawnDelayInterval);
            // spawn for both sides
            Enemy instanceTop = m_spawnerTop.Spawn(spawnRef, parent);
            Enemy instanceBottom = m_spawnerBottom.Spawn(spawnRef, parent);
            // listen to defeat callback
            instanceTop.OnDefeated += OnDefeat;
            instanceBottom.OnDefeated += OnDefeat;
            // add to local enemy list
            m_spawnRefs.Add(instanceTop);
            m_spawnRefs.Add(instanceBottom);
            onSpawnAction?.Invoke(instanceTop);
            onSpawnAction?.Invoke(instanceBottom);
            m_spawnerTop.transform.position += move;
            m_spawnerBottom.transform.position += move;
            numSpawned++;

        }
        // reset positions
        m_spawnerTop.transform.position = m_defaultTop;
        m_spawnerBottom.transform.position = m_defaultBottom;
    }
    // spawn for left & right side
    IEnumerator Spawn_Vertical(Enemy spawnRef, Transform parent, float spawnDelayInterval = 3f, Action<Enemy> onSpawnAction = null)
    {
        int numSpawned = 0;
        float yDist = m_arenaBounds.bounds.max.y - m_arenaBounds.bounds.min.y;
        float distInterval = yDist / m_numToSpawnVerticalSide;
        float moveIntervalPerSpawn = Mathf.Max(1f, distInterval);
        Vector3 move = new Vector3(0f, moveIntervalPerSpawn, 0f);
        while (numSpawned < m_numToSpawnVerticalSide)
        {
            yield return new WaitForSeconds(spawnDelayInterval);
            // spawn for both sides
            Enemy instanceLeft = m_spawnerLeft.Spawn(spawnRef, parent);
            Enemy instanceRight = m_spawnerRight.Spawn(spawnRef, parent);
            instanceLeft.OnDefeated += OnDefeat;
            instanceRight.OnDefeated += OnDefeat;
            m_spawnRefs.Add(instanceLeft);
            m_spawnRefs.Add(instanceRight);

            onSpawnAction?.Invoke(instanceLeft);
            onSpawnAction?.Invoke(instanceRight);
            m_spawnerLeft.transform.position += move;
            m_spawnerRight.transform.position += move;
            numSpawned++;
        }
        // reset positions
        m_spawnerLeft.transform.position = m_defaultLeft;
        m_spawnerRight.transform.position = m_defaultRight;
    }
}
