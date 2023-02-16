namespace GameResources.Control.Economy.ResourcesQuotas.Scripts
{
    using System;
    using System.Collections.Generic;
    using GameResources.Control.Economy.Resources.Scripts;
    using GameResources.Control.Economy.ResourcesHandler.Scripts;

    public class ResourcesQuotas
    {
        public event Action OnCompletedQuota; 
        
        private StageQuota[] _quotas;
        public IReadOnlyList<StageQuota> Quotas => _quotas;

        public int CompletedQuotas { get; private set; }

        public bool IsAllQuotasCompleted => CompletedQuotas >= Quotas.Count;


        public StageQuota CurrentQuota =>
            IsAllQuotasCompleted
                ? null
                : Quotas[CompletedQuotas];

        public StageQuota PreviousQuota =>
            CompletedQuotas == 0
                ? null
                : Quotas[CompletedQuotas - 1];

        public ResourcesQuotas(EconomyResourcesHandler economyResourcesHandler)
        {
            _quotas = ResourcesQuotasFileReader.GetQuotas(economyResourcesHandler);
        }

        public bool TryCompleteQuota(IEnumerable<Resource> currentResources)
        {
            if (IsQuotaComplete(currentResources) == false)
            {
                return false;
            }
            
            ++CompletedQuotas;

            OnCompletedQuota?.Invoke();
            
            return true;
        }

        private bool IsQuotaComplete(IEnumerable<Resource> currentResources)
        {
            if (IsAllQuotasCompleted)
            {
                return false;
            }

            return CurrentQuota.IsQuotaComplete(currentResources);
        }
    }
}
