using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameResources.Location.Island.Scripts
{
    public sealed class LocationGrid
    {
        public readonly float CellSize;

        public Vector2 Size => SizeInCells * new Vector2(CellSize, CellSize);

        public Vector2Int SizeInCells { get; private set; }

        public LocationCell[] Cells { get; private set; }

        private const float EPSILON = 0.01f; 
        
        public LocationGrid(IReadOnlyList<Vector2Int> cellsIndexes, float cellSize)
        {
            CellSize = cellSize;

            SetCells(cellsIndexes);

            SetSizeInCells();
        }
        
        /// <summary>
        /// Get cell if point inside it
        /// </summary>
        /// <param name="point">Point</param>
        /// <param name="pointedLocationCell">Pointed cell</param>
        /// <returns>True if pointed any cell</returns>
        public bool TryGetPointedCell(in Vector3 point, out LocationCell pointedLocationCell)
        {
            if (Mathf.Abs(point.y) > EPSILON)
            {
                pointedLocationCell = null;

                return false;
            }            
            
            var leftDown = new Vector2(point.x, point.z) + Size / 2;

            var isYInGrid = TryGetIndex
            (
                leftDown.y,
                SizeInCells.y, 
                out var i
            );
            
            var isXInGrid = TryGetIndex
            (
                leftDown.x,
                SizeInCells.x,
                out var j
            );

            if (isYInGrid && isXInGrid)
            {
                return TryGetCell(new Vector2Int(j, i), out pointedLocationCell);
            }

            pointedLocationCell = null;

            return false;
        }

        public Vector3 GetCellPosition(Vector2Int index)
        {
            var position = new Vector2((index.x + 0.5f) * CellSize, (index.y + 0.5f) * CellSize);

            position -= Size / 2;

            return new Vector3(position.x, 0, position.y);
        }

        private void SetCells(IReadOnlyList<Vector2Int> cellsIndexes)
        {
            Cells = new LocationCell[cellsIndexes.Count];

            for (var i = 0; i < cellsIndexes.Count; ++i)
            {
                Cells[i] = new LocationCell(this, cellsIndexes[i]);
            }
        }

        private void SetSizeInCells()
        {
            var newSize = Vector2Int.zero;

            foreach (var cellPosition in Cells.Select(x => x.Index))
            {
                if (newSize.x < cellPosition.x + 1)
                {
                    newSize.x = cellPosition.x + 1;
                }

                if (newSize.y < cellPosition.y + 1)
                {
                    newSize.y = cellPosition.y + 1;
                }
            }

            SizeInCells = newSize;
        }

        private bool TryGetIndex
        (
            float point,
            int maxIndex,
            out int index
        )
        {
            index = Mathf.FloorToInt(point / CellSize);

            return index >= 0 && index < maxIndex;
        }

        private bool TryGetCell(Vector2Int index, out LocationCell locationCell)
        {
            foreach (var cell in Cells)
            {
                if (cell.Index != index)
                {
                    continue;
                }

                locationCell = cell;

                return true;
            }

            locationCell = null;

            return false;
        }
    }
}