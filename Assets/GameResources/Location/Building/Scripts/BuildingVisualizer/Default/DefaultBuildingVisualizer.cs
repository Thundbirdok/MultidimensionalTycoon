using GameResources.Control.Scripts;
using UnityEngine;

namespace GameResources.Location.Building.Scripts.BuildingVisualizer.Default
{
    public class DefaultBuildingVisualizer : MonoBehaviour, IBuildingVisualiser
    {
        [SerializeField]
        private Renderer modelRenderer;

        [SerializeField]
        private Color available = Color.green;
        
        [SerializeField]
        private Color notAvailable = Color.red;
        
        #if UNITY_EDITOR

        [field:SerializeField]
        public BuildingData BuildingData { get; private set; }

        #endif
        
        public void SetIsAvailableToBuild(bool isAvailable)
        {
            modelRenderer.material.color = isAvailable ? available : notAvailable;
        }
    }
}
