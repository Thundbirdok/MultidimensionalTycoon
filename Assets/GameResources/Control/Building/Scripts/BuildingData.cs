using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Control.Scripts
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
        private int size = 1;
        public int Size => size;
    }
}
