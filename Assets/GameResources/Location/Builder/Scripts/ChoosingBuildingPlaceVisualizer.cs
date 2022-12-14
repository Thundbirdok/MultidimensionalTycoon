using System.Linq;
using GameResources.Control.Builder.Scripts;
using GameResources.Control.Building.Scripts;
using GameResources.Location.Builder.CellVisualizer.Scripts;
using GameResources.Location.Building.Scripts;
using GameResources.Location.Building.Scripts.Visualizer;
using UnityEngine;
using Zenject;

namespace GameResources.Location.Builder.Scripts
{
    public sealed class ChoosingBuildingPlaceVisualizer : MonoBehaviour
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

        [SerializeField]
        private GameObject checkSpherePrefab;
        
        [SerializeField]
        private ObjectsInteractionsChecker checker;
        
        private GameObject _checkSphere;
        
        private BuildingData _buildingData;

        private GameObject _building;
        private IBuildingVisualiser _buildingVisualiser;

        private Tweener _tweener;

        private GameObject _buildingPrefab;

        private BuildingVisualData _visualData;
        
        private BuilderEventHandler _builderEventHandler;

        private BuildingsVisualDataCollector _buildingsVisualDataCollector;
        
        [Inject]
        private void Construct
        (
            BuilderEventHandler builderEventHandler, 
            BuildingsVisualDataCollector buildingsVisualDataCollector
        )
        {
            _builderEventHandler = builderEventHandler;
            _buildingsVisualDataCollector = buildingsVisualDataCollector;
        }
        
        private void OnEnable()
        {
            _checkSphere = Instantiate(checkSpherePrefab, transform);
            _checkSphere.SetActive(false);
            
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

            _builderEventHandler.OnBuild += PauseVisualize;
            
            cellPointer.OnCellPointed += SetBuildingPosition;
            cellPointer.OnNoCellPointed += PauseVisualize;
        }

        private void Unsubscribe()
        {
            _builderEventHandler.OnStartBuilding -= StartVisualize;
            _builderEventHandler.OnStopBuilding -= StopVisualize;
            
            _builderEventHandler.OnBuild -= PauseVisualize;
            
            cellPointer.OnCellPointed -= SetBuildingPosition;
            cellPointer.OnNoCellPointed -= PauseVisualize;
        }

        private async void StartVisualize(BuildingData building)
        {
            StopVisualize();
            
            _buildingData = building;

            _visualData = _buildingsVisualDataCollector.Visuals
                .FirstOrDefault(x => x.Data == _buildingData);

            if (_visualData == null)
            {
                return;
            }
            
            _buildingPrefab = await _visualData.Visual.LoadAssetAsync<GameObject>().Task;
            
            _building = Instantiate(_buildingPrefab, transform);

            _building.SetActive(false);

            _checkSphere.transform.localScale = building.InteractionRadius * 2 * Vector3.one;
            _checkSphere.transform.SetParent(_building.transform);
            _checkSphere.transform.localPosition = Vector3.zero;

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

            checker.ClearInteractions();
            _checkSphere.transform.SetParent(transform);
            _checkSphere.SetActive(false);
            
            _building.SetActive(false);

            if (_visualData != null)
            {
                _visualData.Visual.ReleaseAsset();
            }

            _buildingPrefab = null;
            
            cellsVisualizer.FadeOutCurrentCells();
            
            Destroy(_building);

            _building = null;
            _buildingVisualiser = null;
        }
        
        private void SetBuildingPosition()
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
                    cellPointer.PointedCell.Index,
                    cellPointer.PointedGrid.Grid,
                    _buildingData.Size,
                    out var cells
                );
            
            _buildingVisualiser.SetIsAvailableToBuild(isValidPosition);

            cellsVisualizer.VisualizeCells(cells, gridTransform);

            checker.CheckBuildingsInSphere(position, _buildingData);

            _checkSphere.SetActive(true);
            _building.SetActive(true);
        }

        private void PauseVisualize(BuildingSlot _) => PauseVisualize();

        private void PauseVisualize()
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
            
            checker.ClearInteractions();
            cellsVisualizer.FadeOutCurrentCells();

            _building.SetActive(false);
            _checkSphere.SetActive(false);
        }
    }
}
