using System.Collections.Generic;
using System.Linq;
using GameResources.Control.Builder.Scripts;
using GameResources.Location.Island.Scripts;
using UnityEngine;
using Zenject;

namespace GameResources.Location.Builder.Scripts
{
    public class BuilderPositionChecker : MonoBehaviour
    {
        private BuilderEventHandler _builderEventHandler;

        [Inject]
        private void Construct(BuilderEventHandler builderEventHandler)
        {
            _builderEventHandler = builderEventHandler;
        }
        
        public bool IsValidPosition
        (
            LocationCell pointedCell, 
            LocationGrid pointedGrid, 
            int size, 
            out IReadOnlyCollection<LocationCell> cells
        )
        {
            if (TryGetCells(pointedCell, pointedGrid, size, out cells) == false)
            {
                return false;
            }

            var isValid = cells.All(cell => cell.IsOccupied == false);
            
            _builderEventHandler.InvokeValidPosition(isValid);

            return isValid;
        }
        
        private static bool TryGetCells
        (
            LocationCell pointedCell, 
            LocationGrid pointedGrid, 
            int size, 
            out IReadOnlyCollection<LocationCell> cells
        )
        {
            int offset;

            if (size % 2 == 0)
            {
                offset = (size / 2) - 1;
            }
            else
            {
                offset = (size - 1) / 2;
            }
            
            var leftDown = pointedCell.Index - new Vector2Int(offset, offset);

            var foundCells = new List<LocationCell>();

            var isAllCellsFound = true;
            
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    var cellIndex = leftDown + new Vector2Int(j, i);
                    
                    if (pointedGrid.TryGetCell(cellIndex, out var locationCell))
                    {
                        foundCells.Add(locationCell);
                        
                        continue;
                    }

                    isAllCellsFound = false;
                }
            }

            cells = foundCells;
            
            return isAllCellsFound;
        }
    }
}
