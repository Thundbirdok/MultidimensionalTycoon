using UnityEngine;

namespace GameResources.Location.Scripts
{
    public sealed class Cell : IInteractable
    {
        public Vector2Int Position { get; }
        
        public bool IsOccupied;

        public IInteractable Interactable;

        public Cell(Vector2Int position)
        {
            Position = position;
        }
        
        public void Interact() => Interactable.Interact();
    }
}
