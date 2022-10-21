using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameResources.Location.Scripts
{
    public sealed class Grid
    {
        public readonly float CellSize;

        public Vector2 Size => SizeInCells * new Vector2(CellSize, CellSize);

        public Vector2Int SizeInCells { get; private set; }

        public Cell[] Cells { get; private set; }

        public Grid(IReadOnlyList<Vector2Int> cellsIndexes, float cellSize)
        {
            CellSize = cellSize;

            SetCells(cellsIndexes);

            SetSizeInCells();
        }
        
        /// <summary>
        /// Get cell if point inside it
        /// </summary>
        /// <param name="point">Point</param>
        /// <param name="pointedCell">Pointed cell</param>
        /// <returns>True if pointed any cell</returns>
        public bool TryGetPointedCell(in Vector3 point, out Cell pointedCell)
        {
            if (point.y != 0)
            {
                pointedCell = null;

                return false;
            }

            var leftDown = new Vector2(point.x, point.z) - Size / 2;

            var isYInGrid = IsPointInGrid
            (
                leftDown.y,
                SizeInCells.y, 
                out var i
            );
            
            var isXInGrid = IsPointInGrid
            (
                leftDown.x,
                SizeInCells.x,
                out var j
            );

            if (isYInGrid && isXInGrid)
            {
                return TryGetCell(new Vector2Int(j, i), out pointedCell);
            }

            pointedCell = null;

            return false;

        }

        private void SetCells(IReadOnlyList<Vector2Int> cellsIndexes)
        {
            Cells = new Cell[cellsIndexes.Count];

            for (var i = 0; i < cellsIndexes.Count; ++i)
            {
                Cells[i] = new Cell(cellsIndexes[i]);
            }
        }

        private void SetSizeInCells()
        {
            var newSize = Vector2Int.zero;

            foreach (var cellPosition in Cells.Select(x => x.Position))
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

        private bool IsPointInGrid
        (
            float point,
            int maxIndex,
            out int index
        )
        {
            index = Mathf.FloorToInt(point / CellSize);

            return index >= 0 && index < maxIndex;
        }

        private bool TryGetCell(Vector2Int index, out Cell pointedCell)
        {
            foreach (var cell in Cells)
            {
                if (cell.Position != index)
                {
                    continue;
                }

                pointedCell = cell;

                return true;
            }

            pointedCell = null;

            return false;
        }
    }
}