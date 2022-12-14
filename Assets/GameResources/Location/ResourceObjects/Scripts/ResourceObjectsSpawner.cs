using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameResources.Location.ResourceObjects.Scripts
{
    using System.Threading.Tasks;
    using GameResources.Location.Builder.Scripts;
    using GameResources.Location.Island.Scripts;
    using Random = UnityEngine.Random;

    public sealed class ResourceObjectsSpawner : MonoBehaviour
    {
        public event Action OnSpawn;
        
        public bool IsSpawned { get; private set; }
        
        [SerializeField]
        private ResourceObjectVisualDataCollector visualDataCollector;

        [SerializeField]
        private BuilderPositionChecker builderPositionChecker;
        
        [SerializeField]
        private List<LocationGridProvider> grids;

        private readonly List<ResourceObject> _resourceObjects = new List<ResourceObject>();
        public IReadOnlyList<ResourceObject> ResourceObjects => _resourceObjects;
        
        private void Start() => Spawn();
        
        private async void Spawn()
        {
            var tasks = 
            (
                from grid in grids 
                from point in grid.Points 
                select Spawn(point, grid)
            )
            .ToList();

            await Task.WhenAll(tasks);

            IsSpawned = true;
            OnSpawn?.Invoke();
        }

        private async Task Spawn(ResourceObjectPoint point, LocationGridProvider grid)
        {
            grid.Grid.TryGetPointedCell(point.transform.localPosition, out var locationCell);
            
            var isValidPosition = builderPositionChecker.IsValidPosition
            (
                locationCell.Index, 
                grid.Grid, 
                point.ObjectData.Size, 
                out var cells
            );

            if (isValidPosition == false)
            {
                return;
            }
            
            OccupyCells(cells);
            
            var visualData = visualDataCollector.Visuals
                .FirstOrDefault(x => x.Data == point.ObjectData);

            if (visualData == null)
            {
                return;
            }

            var visual = await visualData.Visual.InstantiateAsync
                (
                    point.Point
                )
                .Task;

            if (visual.TryGetComponent(out ResourceObjectVisualService visualService))
            {
                visualService.SetModel(Random.Range(0, visualService.Models.Count));
            }
            
            if (visual.TryGetComponent(out ResourceObject resourceObject))
            {
                _resourceObjects.Add(resourceObject);
            }
        }
        
        private static void OccupyCells(in IReadOnlyCollection<LocationCell> selectedCells)
        {
            foreach (var cell in selectedCells)
            {
                cell.Occupy();
            }
        }
    }
}
