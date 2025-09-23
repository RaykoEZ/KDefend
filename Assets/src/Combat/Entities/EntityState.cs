using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public struct EntityState : IEquatable<EntityState>
{
    public EntityProperty Property;
    public Vector2 Position;

    public bool Equals(EntityState other)
    {
        return Position == other.Position &&
            Property.Health == other.Property.Health &&
            Property.KnockbackModifier == other.Property.KnockbackModifier &&
            Property.MoveSpeed == other.Property.MoveSpeed;
    }
}
