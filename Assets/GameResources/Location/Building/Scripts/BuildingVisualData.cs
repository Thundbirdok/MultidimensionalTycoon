using GameResources.Control.Building.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Location.Building.Scripts
{
    [CreateAssetMenu(fileName = "BuildingVisualData", menuName = "Building/VisualData")]
    public sealed class BuildingVisualData : ScriptableObject
    {
        [field: SerializeField]
        public BuildingData Data { get; private set; }

        [field: SerializeField]
        public AssetReference Visual { get; private set; }
    }
}
