namespace GameResources.Control.BuildingPacks
{
    using System.Collections.Generic;
    using GameResources.Control.Builder.Scripts;
    using GameResources.Control.Economy.Resources.Scripts;
    using GameResources.Control.Economy.Resources.Stone;
    using GameResources.Control.Economy.Resources.Wood;
    using GameResources.Control.Economy.ResourcesHandler.Scripts;
    using UnityEngine;
    using Zenject;

    public sealed class BuildingsPacksAdder : MonoBehaviour
    {
        private BuildingsPacksInitializer _buildingsPacksInitializer;

        private AvailableBuildings _availableBuildings;

        private BuilderEventHandler _builderEventHandler;

        private EconomyResourcesHandler _economyResourcesHandler;

        private List<Resource> _packPrice;
        
        [Inject]
        private void Construct
        (
            AvailableBuildings availableBuildings, 
            BuilderEventHandler builderEventHandler, 
            BuildingsDataCollector buildingsDataCollector,
            EconomyResourcesHandler economyResourcesHandler
        )
        {
            _availableBuildings = availableBuildings;
            _builderEventHandler = builderEventHandler;

            _economyResourcesHandler = economyResourcesHandler;
            
            _buildingsPacksInitializer = new BuildingsPacksInitializer(buildingsDataCollector);

            _packPrice = new List<Resource>()
            {
                new Resource(new Wood(), 100),
                new Resource( new Stone(), 50)
            };
            
            _builderEventHandler.OnNoBuildings += AddNewPack;
            _builderEventHandler.OnRequestAddPack += AddNewPack;

            _economyResourcesHandler.OnChangeValue += TryAddPack;
            
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
            _economyResourcesHandler.OnChangeValue -= TryAddPack;
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
            if (_economyResourcesHandler.IsEnoughResources(_packPrice) == false)
            {
                return;
            }

            _economyResourcesHandler.Spend(_packPrice);
            
            AddNewPack();
        }
    }
}
