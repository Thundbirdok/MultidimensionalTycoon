using GameResources.Control.Economy.Resources.Scripts;
using UnityEngine;

namespace GameResources.Control.ResourceObjects.Scripts
{
    [CreateAssetMenu(fileName = "ResourceObject", menuName = "ResourceObject/ResourceObjectData")]
    public class ResourceObjectData : ScriptableObject, IResourceObjectData
    {
        [SerializeField]
        private string key;

        public string Key => key;

        [SerializeField]
        private int size = 1;
        public int Size => size;
        
        public bool TryGetValue(IResourceObjectData data, out Resource value)
        {
            value = null;

            return false;
        }
    }
}
