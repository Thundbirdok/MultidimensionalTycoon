namespace GameResources.Control.Economy.ResourcesQuotas.Scripts
{
    using System;
    using System.Collections.Generic;

    public class ResourcesQuotas
    {
        private List<ResourcesQuota> _quotas;
        public IReadOnlyList<ResourcesQuota> Quotas => _quotas;

        public int CompletedQuotas { get; private set; }

        public ResourcesQuota CurrentQuota => 
            CompletedQuotas < Quotas.Count 
                ? Quotas[CompletedQuotas] 
                : null;
        
        public void CompleteQuota() => ++CompletedQuotas;

        public class ResourcesQuota
        {
            public List<IResourceQuota> quota;
        }

        public interface IResourceQuota
        {
            public string Key { get; }

            public Type ResourceType { get; }

            public int Value { get; }
        }
    }
}
