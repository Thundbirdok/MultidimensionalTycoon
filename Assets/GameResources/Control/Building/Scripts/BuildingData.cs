using System.Collections.Generic;
using System.Linq;
using GameResources.Control.Economy.Resources.Scripts;
using GameResources.Control.ResourceObjects.Scripts;
using UnityEngine;

namespace GameResources.Control.Building.Scripts
{
    using System;

    [CreateAssetMenu(fileName = "Building", menuName = "Building/BuildingData")]
    public sealed class BuildingData : ResourceObjectData, IResourceObjectData
    {
        [SerializeField]
        private float interactionRadius;
        public float InteractionRadius => interactionRadius;
        
        [NonSerialized]
        private List<BuildingsInteractionValue> _interactions = new List<BuildingsInteractionValue>();

        public new bool TryGetInteractionValue(IResourceObjectData data, out Resource value)
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
