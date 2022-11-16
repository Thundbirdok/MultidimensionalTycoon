using UnityEngine;

namespace GameResources.Location.Island.Scripts
{
    public sealed class LocationCell : IInteractable
    {
        public Vector2Int Index { get; }
        
        public bool IsOccupied { get; private set; }

        public IInteractable Interactable;

        private readonly LocationGrid _grid;
        
        public LocationCell(LocationGrid grid, Vector2Int index)
        {
            _grid = grid;
            Index = index;
        }
        
        public void Interact() => Interactable.Interact();

        public Vector3 GetPosition() => _grid.GetCellPosition(Index);

        public void Occupy() => IsOccupied = true;
    }
}
