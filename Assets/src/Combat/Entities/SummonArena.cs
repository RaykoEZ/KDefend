using UnityEngine;
// Summon guards to surround the player
public class SummonArena : ActiveAbility
{
    [SerializeField] Enemy m_arenaSpawnRef = default;
    [SerializeField] BoxSpawner m_spawner = default;
    protected override void Effect_Internal()
    {
        if (m_spawner.SpawnRefs.Count > 0) return;
        m_spawner?.Spawn(m_arenaSpawnRef, transform.parent, 0.05f, PrepareEnemy);
    }
    void PrepareEnemy(Enemy spawned)
    {
        spawned.InitTarget(spawned);
        // set spawned target to move towards arena center
        spawned.Navigator.IgnorePlayerTracking = true;
        spawned.Navigator.DirectDestination = transform.position;
        spawned.Navigator.StartMoving();
    }
}