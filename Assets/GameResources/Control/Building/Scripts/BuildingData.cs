using System.Collections.Generic;
using System.Linq;
using GameResources.Control.Economy.Resources.Scripts;
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
        
        private List<BuildingsInteractionValue> _interactions = new List<BuildingsInteractionValue>();
        public IReadOnlyList<BuildingsInteractionValue> Interactions => _interactions;

        public new bool TryGetValue(IResourceObjectData data, out Resource value)
        {
            var interactionValue = _interactions
                .FirstOrDefault
                (
                    x => x.Data.Equals(data)
                );

            if (interactionValue == null)
            {
                value = null;
                
                return false;
            }

            value = interactionValue.Value;
            
            return true;
        }

        public void SetInteractions(List<BuildingsInteractionValue> interactions)
        {
            _interactions = interactions;
        }
    }
}
