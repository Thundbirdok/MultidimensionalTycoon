using GameResources.Control.Building.Scripts;
using UnityEngine;

namespace GameResources.Location.Building.Scripts.Visualizer
{
    public sealed class BuildingVisualizer : MonoBehaviour, IBuildingVisualiser
    {
        [SerializeField]
        private Renderer[] modelRenderers;

        [SerializeField]
        private Color available = Color.green;
        
        [SerializeField]
        private Color notAvailable = Color.red;
        
        #if UNITY_EDITOR

        [field:SerializeField]
        public BuildingData Data { get; private set; }

        #endif
        
        public void SetIsAvailableToBuild(bool isAvailable)
        {
            foreach (var modelRenderer in modelRenderers)
            {
                modelRenderer.material.color = isAvailable ? available : notAvailable;
            }
        }
    }
}
