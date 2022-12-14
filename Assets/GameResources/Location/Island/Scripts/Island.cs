using System.Collections.Generic;
using GameResources.Location.ResourceObjects.Scripts;
using GameResources.Location.ResourcesInteraction.Scripts;
using UnityEngine;

namespace GameResources.Location.Island.Scripts
{
    using GameResources.Location.Builder.Scripts;

    public sealed class Island : MonoBehaviour
    {
        [SerializeField] 
        private string key;

        public string Key => key;

        [SerializeField]
        private LocationGridProvider[] grids;

        [SerializeField]
        private ResourceObjectsSpawner resourceObjectsSpawner;

        private readonly List<IGiveResources> _resources = new List<IGiveResources>();
        public IReadOnlyList<IGiveResources> Resources => _resources;

        private void Start()
        {
            if (resourceObjectsSpawner.IsSpawned)
            {
                AddResourceObjects();
                
                return;
            }
            
            resourceObjectsSpawner.OnSpawn += AddResourceObjects;
        }

        private void AddResourceObjects()
        {
            resourceObjectsSpawner.OnSpawn -= AddResourceObjects;
            
            _resources.AddRange(resourceObjectsSpawner.ResourceObjects);
        }

        public void AddResource(IGiveResources resource) => _resources.Add(resource);
    }
}
