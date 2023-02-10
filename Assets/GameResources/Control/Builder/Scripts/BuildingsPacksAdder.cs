using UnityEngine;
using Zenject;

namespace GameResources.Control.Builder.Scripts
{
    public sealed class BuildingsPacksAdder : MonoBehaviour
    {
        private BuildingsPacksInitializer _buildingsPacksInitializer;

        private AvailableBuildings _availableBuildings;

        private BuilderEventHandler _builderEventHandler;
        
        [Inject]
        private void Construct
        (
            AvailableBuildings availableBuildings, 
            BuilderEventHandler builderEventHandler, 
            BuildingsDataCollector buildingsDataCollector
        )
        {
            _availableBuildings = availableBuildings;
            _builderEventHandler = builderEventHandler;

            _buildingsPacksInitializer = new BuildingsPacksInitializer(buildingsDataCollector);
            
            _builderEventHandler.OnNoBuildings += AddNewPack;
            _builderEventHandler.OnRequestAddPack += AddNewPack;
            
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
    }
}
