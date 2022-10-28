using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Location.Builder.Scripts
{
    public sealed class Builder : MonoBehaviour
    {
        public event Action<AssetReference> OnStartBuilding;
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
                    OnStartBuilding?.Invoke(buildingReference);
                    
                    return;
                }
                
                OnStopBuilding?.Invoke();
            }
        }
        
        [SerializeField]
        private CellPointer cellPointer;

        [SerializeField]
        private AssetReference buildingReference;

        public AssetReference BuildingReference => buildingReference;
        
        private void OnEnable()
        {
            IsBuilding = true;
        }

        private void Update()
        {
            if (!cellPointer.IsCellPointedNow || !Input.GetMouseButtonDown(0))
            {
                return;
            }

            _ = Build();
        }

        private async Task Build()
        {
            var cell = cellPointer.PointedCell;
            var gridTransform = cellPointer.PointedGrid.transform;

            var building = await buildingReference.InstantiateAsync(gridTransform).Task;

            var localPosition = cell.GetPosition();
            var position = gridTransform.transform.TransformPoint(localPosition);

            building.transform.position = position;
            building.transform.rotation = gridTransform.rotation;

            IsBuilding = false;
        }
    }
}