using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameResources.Location.Scripts
{
    public sealed class GridProvider : MonoBehaviour
    {
        [SerializeField] 
        private float cellSize;

        [SerializeField]
        private List<Vector2Int> cellsPositions = new List<Vector2Int>();

        public Grid Grid { get; private set; }

        private void OnEnable()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            Grid = new Grid(cellsPositions, cellSize);
        }

#if UNITY_EDITOR

        /// <summary>
        /// EDITOR ONLY
        /// Clear duplicates
        /// </summary>
        public void ClearDuplicates()
        {
            cellsPositions = cellsPositions.Distinct().ToList();
        } 
        
        private void OnValidate()
        {
            cellsPositions.Sort(Vector2Comparison);
            
            CreateGrid();
        }

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
