using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameResources.Location.Island.Scripts
{
    using GameResources.Location.ResourceObjects.Scripts;

    public sealed class LocationGridProvider : MonoBehaviour
    {
        [SerializeField] 
        private float cellSize;

        [SerializeField]
        private List<Vector2Int> cellsPositions = new List<Vector2Int>();

        [SerializeField]
        private List<ResourceObjectPoint> points;
        public List<ResourceObjectPoint> Points => points;
        
        public LocationGrid Grid { get; private set; }

        private void OnEnable() => CreateGrid();

        private void CreateGrid() => Grid = new LocationGrid(cellsPositions, cellSize);

#if UNITY_EDITOR

        /// <summary>
        /// EDITOR ONLY
        /// Clear cells duplicates
        /// </summary>
        public void ClearCellDuplicates() => cellsPositions = cellsPositions.Distinct().ToList();

        /// <summary>
        /// EDITOR ONLY
        /// Sort cells
        /// </summary>
        public void SortCells() => cellsPositions.Sort(Vector2Comparison);

        private void OnValidate() => CreateGrid();

        private static int Vector2Comparison(Vector2Int a, Vector2Int b)
        {
            if (a.y != b.y)
            {
                return a.y - b.y;
            }

            return a.x - b.x;
        }

#endif
        
    }
}
