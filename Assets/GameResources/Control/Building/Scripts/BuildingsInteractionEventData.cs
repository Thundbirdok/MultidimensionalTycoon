using System;
using UnityEngine;

namespace GameResources.Control.Building.Scripts
{
    public sealed class BuildingsInteractionEventData: IEquatable<BuildingsInteractionEventData>
    {
        public readonly Vector3 Position;
        public readonly int Value;

        public BuildingsInteractionEventData(Vector3 position, int value)
        {
            Position = position;
            Value = value;
        }

        public bool Equals(BuildingsInteractionEventData other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Position.Equals(other.Position)
                && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is BuildingsInteractionEventData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Value);
        }
    }
}
