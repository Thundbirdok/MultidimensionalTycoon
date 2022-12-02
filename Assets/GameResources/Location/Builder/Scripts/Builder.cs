using System.Collections.Generic;
using GameResources.Control.Builder.Scripts;
using GameResources.Control.Scripts;
using GameResources.Inputs;
using GameResources.Location.Island.Scripts;
using UnityEngine;
using Zenject;

namespace GameResources.Location.Builder.Scripts
{
    public sealed class Builder : MonoBehaviour
    {
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
                    InputStates.IsDragActive = false;
                    _eventHandler.InvokeStartBuilding(BuildingData);
                    
                    return;
                }
                
                InputStates.IsDragActive = true;
                _eventHandler.InvokeStopBuilding();
            }
        }
        
        public BuildingData BuildingData { get; private set; }

        [SerializeField]
        private CellPointer cellPointer;

        [SerializeField]
        private BuilderPositionChecker positionChecker;

        private AvailableBuildings _availableBuildings;
        private BuilderEventHandler _eventHandler;

        [Inject]
        private void Construct(AvailableBuildings availableBuildings, BuilderEventHandler eventHandler)
        {
            _availableBuildings = availableBuildings;
            _eventHandler = eventHandler;
        }
        
        private void OnEnable()
        {
            _eventHandler.OnChooseBuilding += OnChooseBuilding;
            _eventHandler.OnAccept += Build;
            _eventHandler.OnCancel += StopBuilding;
        }

        private void OnDisable()
        {
            _eventHandler.OnChooseBuilding -= OnChooseBuilding;
            _eventHandler.OnAccept -= Build;
            _eventHandler.OnCancel -= StopBuilding;
        }

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

            return positionChecker
                .IsValidPosition
                (
                    cellPointer.PointedCell,
                    cellPointer.PointedGrid.Grid,
                    BuildingData.Size,
                    out cells
                );
        }

        private async void Build()
        {
            if (IsCanBuildHere(out var cells) == false)
            {
                return;
            }

            _availableBuildings.SpendBuilding(BuildingData, out var restAmount);

            if (restAmount == 0)
            {
                IsBuilding = false;
            }
            
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

            OccupyCells(cells);
        }

        private void StopBuilding() => IsBuilding = false;

        private static void OccupyCells(in IReadOnlyCollection<LocationCell> selectedCells)
        {
            foreach (var cell in selectedCells)
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