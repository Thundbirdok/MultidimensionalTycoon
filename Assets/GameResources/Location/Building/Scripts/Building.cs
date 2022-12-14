using System;
using GameResources.Control.Building.Scripts;
using GameResources.Control.ResourceObjects.Scripts;
using GameResources.Location.ResourcesInteraction.Scripts;
using UnityEngine;

namespace GameResources.Location.Building.Scripts
{
    public sealed class Building : MonoBehaviour, IGiveResources
    {
        public Vector3 Position => transform.position;
        public IResourceObjectData ResourceObjectData => data;

        [SerializeField]
        private BuildingData data;

        public bool Equals(Building other) => base.Equals(other) && Equals(data, other.data);

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

            return obj.GetType() == GetType() && Equals((Building)obj);
        }

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), data);
    }
}
