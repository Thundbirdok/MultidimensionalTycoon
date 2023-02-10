using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameResources.Control.Economy.Resources.Scripts
{
    [Serializable]
    public class ResourcesList
    {
        [field: SerializeField]
        public Dictionary<IResourceType, Resource> Resources { get; private set; }

        public ResourcesList()
        {
            Resources = new Dictionary<IResourceType, Resource>();
        }
        
        public ResourcesList(IEnumerable<Resource> resources)
        {
            Resources = new Dictionary<IResourceType, Resource>();
            
            foreach (var resource in resources)
            {
                Add(resource);
            }
        }

        public void Add(Resource resource)
        {
            if (Resources.TryGetValue(resource.Type, out var value))
            {
                value.Value += resource.Value;
                
                return;
            }
            
            Resources.Add(resource.Type, resource);
        }
    }
}
