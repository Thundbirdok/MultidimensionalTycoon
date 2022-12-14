using System;
using GameResources.Control.ResourceObjects.Scripts;
using GameResources.Location.ResourcesInteraction.Scripts;
using UnityEngine;

namespace GameResources.Location.ResourceObjects.Scripts
{
    public sealed class ResourceObject : MonoBehaviour, IGiveResources
    {
        public Vector3 Position => transform.position;
        public IResourceObjectData ResourceObjectData => data;

        [SerializeField]
        private ResourceObjectData data;
        
        public bool Equals(ResourceObject other) => base.Equals(other) && Equals(data, other.data);

        public bool Equals(IGiveResources obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((ResourceObject)obj);
        }

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), data);
    }
}
