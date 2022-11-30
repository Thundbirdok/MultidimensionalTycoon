using GameResources.Control.Scripts;
using UnityEngine;

namespace GameResources.UI.GameUI.Location.Scripts
{
    [CreateAssetMenu(fileName = "BuildingView", menuName = "Building/BuildingView")]
    public class BuildingViewData : ScriptableObject
    {
        public string Key => buildingData.Key;
        
        [SerializeField]
        private Sprite icon;
        public Sprite Icon => icon;

        [SerializeField]
        private BuildingData buildingData;
        public BuildingData BuildingData => buildingData;
    }
}
