using UnityEngine;
using Zenject;

namespace GameResources.Control.Builder.Scripts
{
    public class BuildingsPacksController : MonoBehaviour
    {
        private BuildingsPacksInitializer _buildingsPacksInitializer;

        private AvailableBuildings _availableBuildings;
        
        [Inject]
        private void Construct(AvailableBuildings availableBuildings, BuildingsDataCollector buildingsDataCollector)
        {
            _availableBuildings = availableBuildings;

            _buildingsPacksInitializer = new BuildingsPacksInitializer(buildingsDataCollector);
            
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
        }

        private void AddNewPack()
        {
            _buildingsPacksInitializer.OnInited -= AddNewPack;
            
            var packIndex = UnityEngine.Random.Range(0, _buildingsPacksInitializer.Packs.Count);

            var pack = _buildingsPacksInitializer.Packs[packIndex];
            
            _availableBuildings.AddPack(pack);
        }
    }
}
