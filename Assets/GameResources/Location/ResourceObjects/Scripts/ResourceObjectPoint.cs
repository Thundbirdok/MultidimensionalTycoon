using UnityEngine;

namespace GameResources.Location.ResourceObjects.Scripts
{
    using GameResources.Control.ResourceObjects.Scripts;

    public sealed class ResourceObjectPoint : MonoBehaviour
    {
        public Transform Point => transform;
        
        [field: SerializeField]
        public ResourceObjectData ObjectData { get; private set; }
    }
}
