using GameResources.Control.ResourceObjects.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Location.ResourceObjects.Scripts
{
    [CreateAssetMenu
    (
        fileName = "ResourceObjectVisualData",
        menuName = "ResourceObject/ResourceObjectVisualData"
    )]
    public sealed class ResourceObjectVisualData : ScriptableObject
    {
        [field: SerializeField]
        public ResourceObjectData Data { get; private set; }

        [field: SerializeField]
        public AssetReference Visual { get; private set; }
    }
}
