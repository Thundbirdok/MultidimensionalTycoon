using System;
using System.Threading.Tasks;
using GameResources.Location.Building.Scripts;
using UnityEngine;

namespace GameResources.Location.Builder.Scripts
{
    public sealed class Builder : MonoBehaviour
    {
        public event Action<BuildingData> OnStartBuilding;
        public event Action OnStopBuilding;

        private bool _isBuilding;
        public bool IsBuilding
        {
            get
            {
                return _isBuilding;
            }

            private set
            {
                if (_isBuilding == value)
                {
                    return;
                }

                _isBuilding = value;

                if (_isBuilding)
                {
                    OnStartBuilding?.Invoke(BuildingData);
                    
                    return;
                }
                
                OnStopBuilding?.Invoke();
            }
        }
        
        public BuildingData BuildingData { get; private set; }

        [SerializeField]
        private CellPointer cellPointer;

        [SerializeField]
        private BuilderEventHandler builderEventHandler;

        private bool IsCanBuildHere => IsBuilding && cellPointer.IsCellPointedNow;
        
        private void OnEnable()
        {
            builderEventHandler.OnChooseBuilding += OnChooseBuilding;
        }

        private void Update()
        {
            if (IsCanBuildHere == false || Input.GetMouseButtonDown(0) == false)
            {
                return;
            }

            Build();
        }

        private async void Build()
        {
            var cell = cellPointer.PointedCell;
            var gridTransform = cellPointer.PointedGrid.transform;

            var building = await BuildingData.Model.InstantiateAsync(gridTransform).Task;

            var localPosition = cell.GetPosition();
            var position = gridTransform.transform.TransformPoint(localPosition);

            building.transform.position = position;
            building.transform.rotation = gridTransform.rotation;

            IsBuilding = false;
        }

        private void OnChooseBuilding(BuildingData data)
        {
            BuildingData = data;

            IsBuilding = true;
        }
    }
}