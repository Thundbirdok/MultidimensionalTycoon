namespace GameResources.Control.BuildingPacks
{
    using GameResources.Control.Builder.Scripts;
    using GameResources.Control.Economy.ResourcesHandler.Scripts;
    using GameResources.Control.Economy.ResourcesQuotas.Scripts;
    using UnityEngine;
    using Zenject;

    public sealed class BuildingsPacksAdder : MonoBehaviour
    {
        private BuildingsPacksInitializer _buildingsPacksInitializer;

        private AvailableBuildings _availableBuildings;

        private BuilderEventHandler _builderEventHandler;

        private EconomyResourcesHandler _economyResourcesHandler;

        private ResourcesQuotas _resourcesQuotas;
        
        [Inject]
        private void Construct
        (
            AvailableBuildings availableBuildings, 
            BuilderEventHandler builderEventHandler, 
            BuildingsDataCollector buildingsDataCollector,
            EconomyResourcesHandler economyResourcesHandler,
            ResourcesQuotas resourcesQuotas
        )
        {
            _availableBuildings = availableBuildings;
            _builderEventHandler = builderEventHandler;

            _economyResourcesHandler = economyResourcesHandler;
            
            _buildingsPacksInitializer = new BuildingsPacksInitializer(buildingsDataCollector);

            _resourcesQuotas = resourcesQuotas;
            
            _builderEventHandler.OnNoBuildings += AddNewPack;
            _builderEventHandler.OnRequestAddPack += AddNewPack;

            _economyResourcesHandler.OnChangedValue += TryAddPack;
            
            if (_buildingsPacksInitializer.IsInited)
            {
                AddNewPack();
                
                return;
            }
            
            _buildingsPacksInitializer.OnInited += AddNewPack;
        }

        private void OnDisable()
        {
            _buildingsPacksInitializer.OnInited -= AddNewPack;
            _builderEventHandler.OnNoBuildings -= AddNewPack;
            _builderEventHandler.OnRequestAddPack -= AddNewPack;
            _economyResourcesHandler.OnChangedValue -= TryAddPack;
        }

        private void OnDestroy()
        {
            _buildingsPacksInitializer = null;
        }

        public void AddNewPack()
        {
            _buildingsPacksInitializer.OnInited -= AddNewPack;
            
            var packIndex = UnityEngine.Random.Range(0, _buildingsPacksInitializer.Packs.Count);

            var pack = _buildingsPacksInitializer.Packs[packIndex];
            
            _availableBuildings.AddPack(pack);
        }
        
        private void TryAddPack()
        {
            if (_resourcesQuotas.IsAllQuotasCompleted)
            {
                return;
            }

            if (_resourcesQuotas.TryCompleteQuota(_economyResourcesHandler.Resources))
            {
                _economyResourcesHandler.Spend(_resourcesQuotas.PreviousQuota.Resources.Values);
            }

            AddNewPack();
        }
    }
}
