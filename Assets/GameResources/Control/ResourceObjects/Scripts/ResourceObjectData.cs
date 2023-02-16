using GameResources.Control.Economy.Resources.Scripts;
using UnityEngine;

namespace GameResources.Control.ResourceObjects.Scripts
{
    [CreateAssetMenu(fileName = "ResourceObject", menuName = "ResourceObject/ResourceObjectData")]
    public class ResourceObjectData : ScriptableObject, IResourceObjectData
    {
        [field: SerializeField]
        public string Key { get; private set; }

        [field: SerializeField]
        public int Size { get; private set; } = 1;

        public bool TryGetInteractionValue(IResourceObjectData data, out Resource value)
        {
            value = null;

            return false;
        }
    }
}
