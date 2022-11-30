using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameResources.Control.Builder.Scripts;
using GameResources.UI.LocationUI.Scripts;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace GameResources.UI.GameUI.Location.Scripts
{
    [Serializable]
    public class PopulateBuildingList
    {
        public event Action OnPopulate;
        public event Action OnBeforeClear;
        
        private List<BuildingView> _buildingsViews = new List<BuildingView>();
        public IReadOnlyList<BuildingView> BuildingsViews => _buildingsViews;

        [SerializeField]
        private BuildingView prefab;

        [SerializeField]
        private Transform container;

        [SerializeField]
        private int defaultListCapacity = 4;
        
        [SerializeField]
        private int maxListSize = 8;
        
        private ObjectPool<BuildingView> _pool;

        private AvailableBuildings _availableBuildings;

        private BuildingsViewDataCollector _buildingsViewDataCollector;

        public void Construct
        (
            AvailableBuildings availableBuildings, 
            BuildingsViewDataCollector buildingsViewDataCollector
        )
        {
            _availableBuildings = availableBuildings;
            _buildingsViewDataCollector = buildingsViewDataCollector;

            InitPool();

            _availableBuildings.OnAddPack += Populate;
            
            Populate();
        }

        ~PopulateBuildingList()
        {
            _availableBuildings.OnAddPack -= Populate;
        }

        private void InitPool()
        {
            _pool = new ObjectPool<BuildingView>
            (
                CreateView, 
                GetView, 
                ReleaseView, 
                DestroyView, 
                false, 
                defaultListCapacity, 
                maxListSize
            );
        }
        
        private async void Populate()
        {
            Clear();
            
            foreach (var slot in _availableBuildings.Slots)
            {
                var view = _pool.Get();

                SetView(slot, view);

                await Task.Yield();
            }
            
            OnPopulate?.Invoke();
        }

        private void SetView(BuildingSlot slot, BuildingView view)
        {
            foreach (var viewData in _buildingsViewDataCollector.Views)
            {
                if (viewData.BuildingData == slot.Data)
                {
                    view.Set(viewData, slot.Amount);

                    _buildingsViews.Add(view);
                    
                    return;
                }
            }

            Debug.LogError("Compared view and data not found");
        }

        private void Clear()
        {
            OnBeforeClear?.Invoke();
            
            foreach (var buildingView in _buildingsViews)
            {
                _pool.Release(buildingView);
            }
            
            _buildingsViews.Clear();
        }

        private BuildingView CreateView() => Object.Instantiate(prefab, container);

        private void GetView(BuildingView view)
        {
            view.transform.SetAsLastSibling();
            view.gameObject.SetActive(true);
        }

        private void ReleaseView(BuildingView view) => view.gameObject.SetActive(false);

        private void DestroyView(BuildingView view) => Object.Destroy(view.gameObject);
    }
}
