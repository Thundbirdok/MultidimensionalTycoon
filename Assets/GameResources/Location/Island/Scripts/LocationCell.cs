using GameResources.Location.Scripts;
using UnityEngine;

namespace GameResources.Location.Island.Scripts
{
    public sealed class LocationCell : IInteractable
    {
        public Vector2Int Index { get; }
        
        public bool IsOccupied;

        public IInteractable Interactable;

        private readonly LocationGrid _grid;
        
        public LocationCell(LocationGrid grid, Vector2Int index)
        {
            _grid = grid;
            Index = index;
        }
        
        public void Interact() => Interactable.Interact();

        public Vector3 GetPosition()
        {
            return _grid.GetCellPosition(Index);
        }
    }
}
