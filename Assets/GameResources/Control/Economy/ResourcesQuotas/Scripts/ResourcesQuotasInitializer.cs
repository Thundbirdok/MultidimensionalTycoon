namespace GameResources.Control.Economy.ResourcesQuotas.Scripts
{
    using System;
    using GameResources.Control.Economy.ResourcesHandler.Scripts;
    using UnityEngine;
    using Zenject;

    [CreateAssetMenu(fileName = "ResourcesQuotasHandler", menuName = "Economy/ResourcesQuotasHandler")]
    public class ResourcesQuotasInitializer : ScriptableObjectInstaller
    {
        public event Action OnInited;
        
        public bool IsInited { get; private set; }
        
        private ResourcesQuotas _quotas;
        
        public override void InstallBindings()
        {
            if (IsInited)
            {
                Container.Bind<ResourcesQuotas>().FromInstance(_quotas);
                
                return;
            }
            
            _quotas = new ResourcesQuotas(Container.Resolve<EconomyResourcesHandler>());

            Container.Bind<ResourcesQuotas>().FromInstance(_quotas);

            IsInited = true;
            OnInited?.Invoke();
        }
    }
}
