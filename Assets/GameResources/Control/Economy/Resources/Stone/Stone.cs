using System;
using GameResources.Control.Economy.Resources.Scripts;

namespace GameResources.Control.Economy.Resources.Stone
{
    public class Stone : IResourceType
    {
        public string Key => "Stone";
        
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
    }
}
