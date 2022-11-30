using GameResources.Control.Scripts;
using GameResources.Location.Builder.CellVisualizer.Scripts;
using GameResources.Location.Building.Scripts;
using GameResources.Location.Building.Scripts.BuildingVisualizer;
using UnityEngine;
using Zenject;

namespace GameResources.Location.Builder.Scripts
{
    public class ChoosingBuildingPlaceVisualizer : MonoBehaviour
    {
        [SerializeField]
        private Builder builder;

        [SerializeField]
        private BuilderPositionChecker positionChecker;
        
        [SerializeField]
        private CellPointer cellPointer;

        [SerializeField]
        private CellsVisualizer cellsVisualizer;

        [SerializeField]
        private float tweenDuration = 0.5f;

        private BuildingData _buildingData;

        private GameObject _building;
        private IBuildingVisualiser _buildingVisualiser;

        private Tweener _tweener;

        private GameObject _buildingPrefab;

        private BuilderEventHandler _builderEventHandler;

        [Inject]
        private void Construct(BuilderEventHandler builderEventHandler)
        {
            _builderEventHandler = builderEventHandler;
        }
        
        private void OnEnable()
        {
            if (builder.IsBuilding)
            {
                StartVisualize(builder.BuildingData);
            }
            
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
            StopVisualize();
        }

        private void Update() => cellsVisualizer.Move();

        private void Subscribe()
        {
            _builderEventHandler.OnStartBuilding += StartVisualize;
            _builderEventHandler.OnStopBuilding += StopVisualize;
            
            cellPointer.OnCellPointed += OnCellPointed;
            cellPointer.OnNoCellPointed += OnNoCellPointed;
        }

        private void Unsubscribe()
        {
            _builderEventHandler.OnStartBuilding -= StartVisualize;
            _builderEventHandler.OnStopBuilding -= StopVisualize;
            
            cellPointer.OnCellPointed -= OnCellPointed;
            cellPointer.OnNoCellPointed -= OnNoCellPointed;
        }

        private async void StartVisualize(BuildingData building)
        {
            _buildingData = building;

            _buildingPrefab = await _buildingData.Model.LoadAssetAsync<GameObject>().Task;
            
            _building = Instantiate(_buildingPrefab, transform);
            
            _building.SetActive(false);
            
            SetBuildingVisualizerComponent();

            _tweener = new Tweener(this, _building, tweenDuration);
        }

        private void SetBuildingVisualizerComponent()
        {
            if (_building.TryGetComponent(out IBuildingVisualiser buildingVisualiser) == false)
            {
                Debug.LogError("No building visualizer component");
                
                return;
            }
            
            _buildingVisualiser = buildingVisualiser;
        }

        private void StopVisualize()
        {
            _tweener = null;
            
            if (_building == null)
            {
                return;
            }

            _building.SetActive(false);
            _buildingData.Model.ReleaseAsset();
            _buildingPrefab = null;
            
            cellsVisualizer.FadeOutCurrentCells();
            
            Destroy(_building);

            _building = null;
            _buildingVisualiser = null;
        }
        
        private void OnCellPointed()
        {
            if (builder.IsBuilding == false)
            {
                return;
            }

            if (_buildingVisualiser == null)
            {
                return;
            }
            
            var cell = cellPointer.PointedCell;
            var gridTransform = cellPointer.PointedGrid.transform;

            var localPosition = cell.GetPosition();

            if (_buildingData.Size % 2 == 0)
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

            _tweener.SetPosition(gridTransform, position);

            var isValidPosition = positionChecker
                .IsValidPosition
                (
                    cellPointer.PointedCell,
                    cellPointer.PointedGrid.Grid,
                    _buildingData.Size,
                    out var cells
                );
            
            _buildingVisualiser.SetIsAvailableToBuild(isValidPosition);

            cellsVisualizer.VisualizeCells(cells, gridTransform);
            
            _building.SetActive(true);
        }

        private void OnNoCellPointed()
        {
            if (builder.IsBuilding == false)
            {
                return;
            }

            if (_tweener != null)
            {
                _tweener.IsTweening = false;
            }

            if (_building == null)
            {
                return;
            }
            
            cellsVisualizer.FadeOutCurrentCells();

            _building.SetActive(false);
        }
    }
}
