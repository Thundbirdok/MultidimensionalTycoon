using GameResources.Location.Building.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Location.Builder.Scripts
{
    public class ChoosingBuildingPlaceVisualizer : MonoBehaviour
    {
        [SerializeField]
        private Builder builder;
        
        [SerializeField]
        private CellPointer cellPointer;
        
        [SerializeField]
        private float tweenDuration = 0.5f;
        
        private BuildingData _buildingData;

        private GameObject _building;

        private Tweener _tweener;
        
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

        private void Subscribe()
        {
            builder.OnStartBuilding += StartVisualize;
            builder.OnStopBuilding += StopVisualize;
            
            cellPointer.OnCellPointed += OnCellPointed;
            cellPointer.OnNoCellPointed += OnNoCellPointed;
        }

        private void Unsubscribe()
        {
            builder.OnStartBuilding -= StartVisualize;
            builder.OnStopBuilding -= StopVisualize;
            
            cellPointer.OnCellPointed -= OnCellPointed;
            cellPointer.OnNoCellPointed -= OnNoCellPointed;
        }

        private async void StartVisualize(BuildingData building)
        {
            _buildingData = building;
            
            _building = await _buildingData.Model.InstantiateAsync().Task;
            _building.SetActive(false);

            _tweener = new Tweener(this, _building, tweenDuration);
        }

        private void StopVisualize()
        {
            _tweener = null;
            
            if (_building == null)
            {
                return;
            }

            _building.SetActive(false);
            _buildingData.Model.ReleaseInstance(_building);
            _building = null;
        }
        
        private void OnCellPointed()
        {
            if (builder.IsBuilding == false)
            {
                return;
            }

            if (_building == null)
            {
                return;
            }
            
            var cell = cellPointer.PointedCell;
            var gridTransform = cellPointer.PointedGrid.transform;

            var localPosition = cell.GetPosition();
            var position = gridTransform.transform.TransformPoint(localPosition);

            _tweener.SetPosition(gridTransform, position);

            _building.SetActive(true);
        }

        private void OnNoCellPointed()
        {
            if (builder.IsBuilding == false)
            {
                return;
            }
            
            _tweener.IsTweening = false;
            
            if (_building == null)
            {
                return;
            }
            
            _building.SetActive(false);
        }
    }
}
