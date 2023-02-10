using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameResources.Control.Builder.Scripts;
using GameResources.Control.Building.Scripts;
using GameResources.Control.Economy.Resources.Scripts;
using GameResources.Control.Economy.ResourcesHandler.Scripts;
using GameResources.Inputs;
using GameResources.Location.Building.Scripts;
using GameResources.Location.Island.Scripts;
using GameResources.Location.ResourcesInteraction.Scripts;
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
        private Island.Scripts.Island island;
        
        [SerializeField]
        private CellPointer cellPointer;

        [SerializeField]
        private BuilderPositionChecker positionChecker;
        
        private BuildingsInteractedValuesHandler _buildingsInteractedValuesHandler;
        
        private AvailableBuildings _availableBuildings;
        private BuilderEventHandler _eventHandler;

        private BuildingsVisualDataCollector _buildingsVisualDataCollector;
        
        private BuildingVisualData _visualData;

        private EconomyResourcesHandler _economyResourceHandler;
        
        [Inject]
        private void Construct
        (
            AvailableBuildings availableBuildings,
            BuilderEventHandler eventHandler,
            BuildingsInteractedValuesHandler buildingsInteractedValuesHandler,
            BuildingsVisualDataCollector buildingsVisualDataCollector,
            EconomyResourcesHandler economyResourcesHandler
        )
        {
            _availableBuildings = availableBuildings;
            _eventHandler = eventHandler;
            _buildingsInteractedValuesHandler = buildingsInteractedValuesHandler;

            _buildingsVisualDataCollector = buildingsVisualDataCollector;
            
            _economyResourceHandler = economyResourcesHandler;
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
                    cellPointer.PointedCell.Index,
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

            var building = await GetBuilding();

            OccupyCells(cells);

            AddBuildingToIsland(building);

            AddValues();
            
            _availableBuildings.SpendBuilding(BuildingData, out var restAmount);

            if (restAmount == 0)
            {
                IsBuilding = false;
            }
        }

        private async Task<GameObject> GetBuilding()
        {
            var gridTransform = cellPointer.PointedGrid.transform;

            var position = GetPosition(cellPointer.PointedCell.GetPosition(), gridTransform);

            return await _visualData.Visual
                .InstantiateAsync(position, gridTransform.rotation, gridTransform)
                .Task;
        }

        private void AddBuildingToIsland(GameObject building)
        {
            if (building.TryGetComponent(out IGiveResources resource))
            {
                island.AddResource(resource);
            }
        }

        private void AddValues()
        {
            var resources =
                _buildingsInteractedValuesHandler.EventsData.SelectMany(eventData => eventData.Value.Resources.Values);

            var resourcesList = new ResourcesList();

            foreach (var resource in resources)
            {
                resourcesList.Add(resource);
            }
            
            foreach (var resource in resourcesList.Resources)
            {
                if (_economyResourceHandler.TryGetHandler(resource.Key, out var handler))
                {
                    handler.Add(resource.Value.Value);
                }
            }
        }

        private Vector3 GetPosition(Vector3 localPosition, Transform gridTransform)
        {
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

            return position;
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
            StopBuilding();
            
            BuildingData = data;

            _visualData = _buildingsVisualDataCollector.Visuals
                .FirstOrDefault(x => x.Data == BuildingData);

            IsBuilding = true;
        }
    }
}