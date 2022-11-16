using System;
using System.Collections.Generic;
using GameResources.Location.Building.Scripts;
using GameResources.Location.Island.Scripts;
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

        private IReadOnlyCollection<LocationCell> _selectedCells;

        private bool IsCanBuildHere(out IReadOnlyCollection<LocationCell> cells)
        {
            cells = null;
            
            if (IsBuilding == false)
            {
                return false;
            }

            if (cellPointer.IsCellPointedNow == false)
            {
                return false;
            }

            return BuilderPositionChecker
                .IsValidPosition
                (
                    cellPointer.PointedCell,
                    cellPointer.PointedGrid.Grid,
                    BuildingData.Size,
                    out cells
                );
        }

        private void OnEnable()
        {
            builderEventHandler.OnChooseBuilding += OnChooseBuilding;
        }

        private void Update()
        {
            if (IsCanBuildHere(out var cells) == false 
                || Input.GetMouseButtonDown(0) == false)
            {
                return;
            }

            _selectedCells = cells;
            
            Build();
        }

        private async void Build()
        {
            var cell = cellPointer.PointedCell;
            var gridTransform = cellPointer.PointedGrid.transform;

            var localPosition = cell.GetPosition();
            
            if (BuildingData.Size % 2 == 0)
            {
                var axisHalfCellOffset = cellPointer.PointedGrid.Grid.CellSize / 2; 
                
                var halfCellOffset = new Vector3
                (
                    axisHalfCellOffset,
                    0,
                    axisHalfCellOffset
                );

                localPosition += halfCellOffset;
            }
            
            var position = gridTransform.TransformPoint(localPosition);

            var building = await BuildingData.Model
                .InstantiateAsync(position, gridTransform.rotation, gridTransform)
                .Task;

            OccupyCells();
            
            IsBuilding = false;
        }

        private void OccupyCells()
        {
            foreach (var cell in _selectedCells)
            {
                cell.Occupy();
            }
        }

        private void OnChooseBuilding(BuildingData data)
        {
            BuildingData = data;

            IsBuilding = true;
        }
    }
}