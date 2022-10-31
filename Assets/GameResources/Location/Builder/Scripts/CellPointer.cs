using System;
using GameResources.Location.Island.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameResources.Location.Builder.Scripts
{
    public class CellPointer : MonoBehaviour
    {
        public event Action OnCellPointed;
        public event Action OnNoCellPointed; 

        public bool IsCellPointedNow { get; private set; }

        public LocationGridProvider PointedGrid { get; private set; }
        
        private LocationCell _pointedCell;
        public LocationCell PointedCell
        {
            get
            {
                return _pointedCell;
            }

            private set
            {
                IsCellPointedNow = true;
                
                if (_pointedCell == value)
                {
                    return;
                }

                _pointedCell = value;
                
                OnCellPointed?.Invoke();
            }
        }
        
        [SerializeField] 
        private Camera raycastCamera;

        private const float MAX_RAYCAST_DISTANCE = 50;
        private const float MAX_SQR_MAGNITUDE = MAX_RAYCAST_DISTANCE * MAX_RAYCAST_DISTANCE;
        
        private readonly RaycastHit[] _hits = new RaycastHit[5];

        private void Update() => GetPointedCell();

        private void GetPointedCell()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                MarkNoCellPointed();
                
                return;
            }
            
            if (TryGetPointedCell(out var grid, out var cell))
            {
                PointedGrid = grid;
                PointedCell = cell;

                return;
            }

            MarkNoCellPointed();
        }

        private bool TryGetPointedCell(out LocationGridProvider grid, out LocationCell locationCell)
        {
            var ray = raycastCamera.ScreenPointToRay(Input.mousePosition);

            var size = Physics.RaycastNonAlloc(ray, _hits, MAX_RAYCAST_DISTANCE);
            
            if (size == 0)
            {
                grid = null;
                locationCell = null;
                
                return false;
            }
            
            if (TryGetHitOnGrid(_hits, size, out grid, out var hit) == false)
            {
                locationCell = null;
                
                return false;
            }

            var localPoint = grid.transform.InverseTransformPoint(hit.point);

            return grid.LocationGrid.TryGetPointedCell(localPoint, out locationCell);
        }

        private bool TryGetHitOnGrid
        (
            in RaycastHit[] hits, 
            in int size, 
            out LocationGridProvider grid, 
            out RaycastHit hit
        )
        {
            grid = null;
            hit = new RaycastHit();
            
            var closestGridHitSqrMagnitude = MAX_SQR_MAGNITUDE;

            for (var i = 0; i < size; ++i)
            {
                var hitSqrMagnitude = CheckHit(hits[i], out var possibleGrid); 
                
                if (hitSqrMagnitude > closestGridHitSqrMagnitude)
                {
                    continue;
                }

                hit = hits[i];
                grid = possibleGrid;
                closestGridHitSqrMagnitude = hitSqrMagnitude;
            }

            return MAX_SQR_MAGNITUDE - closestGridHitSqrMagnitude > float.Epsilon;
        }

        private float CheckHit
        (
            in RaycastHit hit,
            out LocationGridProvider grid
        )
        {
            var hitted = hit.collider.gameObject;

            if (hitted.TryGetComponent(out grid) == false)
            {
                return 0;
            }

            var cameraPosition = raycastCamera.transform.position;
            var sqrMagnitude = Vector3.SqrMagnitude(hit.point - cameraPosition);
            
            return sqrMagnitude;
        }
        
        private void MarkNoCellPointed()
        {
            if (!IsCellPointedNow)
            {
                return;
            }

            IsCellPointedNow = false;
            OnNoCellPointed?.Invoke();
        }
    }
}
