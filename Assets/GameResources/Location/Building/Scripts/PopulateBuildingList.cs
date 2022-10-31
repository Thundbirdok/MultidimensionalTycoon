using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameResources.Location.Builder.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameResources.Location.Building.Scripts
{
    [Serializable]
    public class PopulateBuildingList
    {
        public event Action OnPopulate;
        
        [SerializeField]
        private BuildingsPricesInitializer buildingsPricesInitializer;

        [SerializeField]
        private BuildingView prefab;

        [SerializeField]
        private Transform container;

        private BuildingsViewDataCollector _buildingsViewDataCollector;

        private List<BuildingView> _buildingsViews = new List<BuildingView>();
        public IReadOnlyList<BuildingView> BuildingsViews => _buildingsViews;
        
        public void Construct(BuildingsViewDataCollector buildingsViewDataCollector)
        {
            _buildingsViewDataCollector = buildingsViewDataCollector;
            
            if (buildingsPricesInitializer.IsInited)
            {
                Populate();
                
                return;
            }

            buildingsPricesInitializer.OnInited += Populate;
        }

        ~PopulateBuildingList()
        {
            buildingsPricesInitializer.OnInited -= Populate;
        }

        private async void Populate()
        {
            buildingsPricesInitializer.OnInited -= Populate;

            foreach (var building in _buildingsViewDataCollector.Buildings)
            {
                var view = Object.Instantiate(prefab, container);

                view.Set(building);
                
                _buildingsViews.Add(view);

                await Task.Yield();
            }
            
            OnPopulate?.Invoke();
        }
    }
}
