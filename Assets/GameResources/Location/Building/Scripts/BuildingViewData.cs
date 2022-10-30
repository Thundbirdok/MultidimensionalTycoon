using System.Collections.Generic;
using GameResources.Economy.Resources.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Location.Building.Scripts
{
    [CreateAssetMenu(fileName = "Building", menuName = "Building/BuildingListElement")]
    public class BuildingViewData : ScriptableObject
    {
        public string Key => buildingData.Key;
        
        [SerializeField]
        private Sprite icon;
        public Sprite Icon => icon;

        [SerializeField]
        private BuildingData buildingData;
        
        public AssetReference Model => buildingData.Model;
        public uint Size => buildingData.Size;
        
        public List<ResourceValue> Price { get; private set; }

        public void SetPrice(in List<ResourceValue> price)
        {
            Price = price;
        }
    }
}
