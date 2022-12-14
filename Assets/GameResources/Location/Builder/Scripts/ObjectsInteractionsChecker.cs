using System;
using System.Collections.Generic;
using System.Linq;
using GameResources.Control.Builder.Scripts;
using GameResources.Control.Building.Scripts;
using GameResources.Location.ResourcesInteraction.Scripts;
using UnityEngine;

namespace GameResources.Location.Builder.Scripts
{
    [Serializable]
    public sealed class ObjectsInteractionsChecker
    {
        [SerializeField]
        private Island.Scripts.Island island;
        
        [SerializeField]
        private BuildingsInteractedValuesHandler handler;

        public void CheckBuildingsInSphere(Vector3 position, BuildingData buildingData)
        {
            var resources = GetIGiveResourcesObjectsInRadius
            (
                position, 
                buildingData.InteractionRadius
            );

            SetInteractionValuesPositions(buildingData, resources);
        }

        public void ClearInteractions() => handler.Clear();

        private void SetInteractionValuesPositions
        (
            BuildingData buildingData, 
            IEnumerable<IGiveResources> resourcesInRadius
        )
        {
            var interactionValuePositions = new List<BuildingsInteractionEventData>();
            
            foreach (var resourceObject in resourcesInRadius)
            {
                var isInteraction = false;
                
                if (buildingData.TryGetValue(resourceObject.ResourceObjectData, out var valueA))
                {
                    isInteraction = true;
                }

                if (resourceObject.ResourceObjectData.TryGetValue(buildingData, out var valueB))
                {
                    isInteraction = true;
                }

                if (isInteraction == false)
                {
                    continue;
                }
                
                var eventData = new BuildingsInteractionEventData(resourceObject.Position, valueA + valueB);

                interactionValuePositions
                    .Add(eventData);
            }

            handler.EventsData = interactionValuePositions;
        }
        
        private IEnumerable<IGiveResources> GetIGiveResourcesObjectsInRadius(Vector3 position, float radius)
        {
            var sqrRadius = radius * radius;

            return island.Resources
                .Where
                (
                    x => Vector3.SqrMagnitude(x.Position - position) <= sqrRadius
                );
        }
    }
}
