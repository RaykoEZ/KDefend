using System;

[Serializable]
public struct EnemyState : IEquatable<EnemyState>
{
    [NonSerialized] public int EnemyIndex;
    public EntityState State;
    public bool Equals(EnemyState other)
    {
        return EnemyIndex == other.EnemyIndex && State.Equals(other.State);
    }
}
