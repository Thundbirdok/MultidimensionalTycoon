using System.Collections.Generic;
using System.Linq;
using GameResources.Control.ResourceObjects.Scripts;
using UnityEngine;

namespace GameResources.Control.Building.Scripts
{
    [CreateAssetMenu(fileName = "Building", menuName = "Building/BuildingData")]
    public sealed class BuildingData : ResourceObjectData, IResourceObjectData
    {
        [SerializeField]
        private float interactionRadius;
        public float InteractionRadius => interactionRadius;
        
        [Tooltip("No zero-value interaction")]
        [SerializeField]
        private List<BuildingsInteractionValue> interactions = new List<BuildingsInteractionValue>();
        
        public new bool TryGetValue(IResourceObjectData data, out int value)
        {
            var interactionValue = interactions.FirstOrDefault(x => x.Data.Equals(data));

            if (interactionValue == null)
            {
                value = 0;
                
                return false;
            }

            value = interactionValue.Value;
            
            return true;
        }
    }
}
