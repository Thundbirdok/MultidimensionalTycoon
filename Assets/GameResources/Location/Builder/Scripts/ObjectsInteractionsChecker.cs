using System;
using System.Collections.Generic;
using System.Linq;
using GameResources.Control.Builder.Scripts;
using GameResources.Control.Building.Scripts;
using GameResources.Control.Economy.Resources.Scripts;
using GameResources.Control.ResourceObjects.Scripts;
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
            IResourceObjectData buildingData, 
            IEnumerable<IGiveResources> resourcesInRadius
        )
        {
            var interactionValuePositions = new List<BuildingsInteractionEventData>();
            
            foreach (var resourceObject in resourcesInRadius)
            {
                AddInteractionValue(buildingData, resourceObject, ref interactionValuePositions);
            }

            handler.EventsData = interactionValuePositions;
        }

        private static void AddInteractionValue
        (
            IResourceObjectData buildingData, 
            IGiveResources resourceObject,
            ref List<BuildingsInteractionEventData> interactionValuePositions
        )
        {
            var isInteraction = false;

            var resources = new ResourcesList();
            
            if (buildingData.TryGetValue(resourceObject.ResourceObjectData, out var valueA))
            {
                resources.Add(valueA);
                
                isInteraction = true;
            }

            if (resourceObject.ResourceObjectData.TryGetValue(buildingData, out var valueB))
            {
                resources.Add(valueB);
                
                isInteraction = true;
            }

            if (isInteraction == false)
            {
                return;
            }

            var eventData = new BuildingsInteractionEventData(resourceObject.Position, resources);

            interactionValuePositions.Add(eventData);
        }

        private IEnumerable<IGiveResources> GetIGiveResourcesObjectsInRadius
        (
            Vector3 position,
            float radius
        )
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
