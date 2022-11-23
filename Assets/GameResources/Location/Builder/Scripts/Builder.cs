using System.Collections.Generic;
using GameResources.Inputs;
using GameResources.Location.Building.Scripts;
using GameResources.Location.Island.Scripts;
using UnityEngine;

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
                    eventHandler.InvokeStartBuilding(BuildingData);
                    
                    return;
                }
                
                InputStates.IsDragActive = true;
                eventHandler.InvokeStopBuilding();
            }
        }
        
        public BuildingData BuildingData { get; private set; }

        [SerializeField]
        private CellPointer cellPointer;

        [SerializeField]
        private BuilderEventHandler eventHandler;

        [SerializeField]
        private BuilderPositionChecker positionChecker;
        
        private void OnEnable()
        {
            eventHandler.OnChooseBuilding += OnChooseBuilding;
            eventHandler.OnAccept += Build;
            eventHandler.OnCancel += StopBuilding;
        }

        private void OnDisable()
        {
            eventHandler.OnChooseBuilding -= OnChooseBuilding;
            eventHandler.OnAccept -= Build;
            eventHandler.OnCancel -= StopBuilding;
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
            
            IsBuilding = false;
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