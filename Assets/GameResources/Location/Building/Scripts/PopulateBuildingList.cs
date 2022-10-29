using UnityEngine;
using Zenject;

namespace GameResources.Location.Building.Scripts
{
    public class PopulateBuildingList : MonoBehaviour
    {
        [SerializeField]
        private BuildingsPricesInitializer buildingsPricesInitializer;

        [SerializeField]
        private BuildingView prefab;

        [SerializeField]
        private Transform container;
        
        private BuildingsViewDataCollector _buildingsViewDataCollector;
        
        [Inject]
        private void Construct(BuildingsViewDataCollector buildingsViewDataCollector)
        {
            _buildingsViewDataCollector = buildingsViewDataCollector;
        }
        
        private void Start()
        {
            if (buildingsPricesInitializer.IsInited)
            {
                Populate();
                
                return;
            }

            buildingsPricesInitializer.OnInited += Populate;
        }

        private void OnDestroy()
        {
            buildingsPricesInitializer.OnInited -= Populate;
        }

        private void Populate()
        {
            buildingsPricesInitializer.OnInited -= Populate;

            foreach (var building in _buildingsViewDataCollector.Buildings)
            {
                var view = Instantiate(prefab, container);
                
                view.Set(building.Icon, building.Price);
            }
        }
    }
}
