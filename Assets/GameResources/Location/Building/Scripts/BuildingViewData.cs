using System.Collections.Generic;
using GameResources.Economy.Resources.Scripts;
using UnityEngine;

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

        public BuildingData BuildingData => buildingData;

        public List<ResourceValue> Price { get; private set; }

        public void SetPrice(in List<ResourceValue> price) => Price = price;
    }
}
