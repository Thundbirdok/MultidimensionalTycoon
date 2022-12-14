using System;
using GameResources.Control.ResourceObjects.Scripts;
using UnityEngine;

namespace GameResources.Location.ResourcesInteraction.Scripts
{
    public interface IGiveResources : IEquatable<IGiveResources>
    {
        public Vector3 Position { get; }
        public IResourceObjectData ResourceObjectData { get; }
    }
}
