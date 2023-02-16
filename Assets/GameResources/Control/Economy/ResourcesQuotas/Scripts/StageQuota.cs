using System.Collections.Generic;
using GameResources.Control.Economy.Resources.Scripts;

namespace GameResources.Control.Economy.ResourcesQuotas.Scripts
{
    public class StageQuota
    {
        public Dictionary<IResourceType, Resource> Resources { get; private set; }

        public StageQuota(IEnumerable<Resource> resources)
        {
            Resources = new Dictionary<IResourceType, Resource>();
                
            foreach (var resource in resources)
            {
                Resources.Add(resource.Type, resource);
            }
        }
            
        public bool IsQuotaComplete(IEnumerable<Resource> currentResources)
        {
            foreach (var resource in currentResources)
            {
                if (Resources.TryGetValue(resource.Type, out var quotaResource) == false)
                {
                    continue;
                }

                if (resource.Value < quotaResource.Value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
