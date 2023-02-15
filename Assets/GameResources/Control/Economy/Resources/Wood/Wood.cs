using System;
using GameResources.Control.Economy.Resources.Scripts;

namespace GameResources.Control.Economy.Resources.Wood
{
    public class Wood : IResourceType
    {
        public string Key => "Wood";

        bool IEquatable<IResourceType>.Equals(IResourceType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return other.GetType() == GetType();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == GetType();
        }

        protected bool Equals(Wood other) => true;

        public override int GetHashCode() => GetType().GetHashCode();
    }
}
