using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Location.Building.Scripts
{
    [CreateAssetMenu(fileName = "Building", menuName = "Building/BuildingData")]
    public class BuildingData : ScriptableObject
    {
        [SerializeField]
        private string key;

        public string Key => key;
        
        [SerializeField]
        private AssetReference model;
        public AssetReference Model => model;

        [SerializeField]
        private uint size = 1;
        public uint Size => size;
    }
}
