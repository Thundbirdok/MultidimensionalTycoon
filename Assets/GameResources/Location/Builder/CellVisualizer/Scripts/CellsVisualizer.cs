using System;
using System.Collections.Generic;
using GameResources.Location.Island.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameResources.Location.Builder.CellVisualizer.Scripts
{
    [Serializable]
    public class CellsVisualizer
    {
        [SerializeField]
        private CellVisualizer cellPrefab;

        [SerializeField]
        private Transform container;
        
        [SerializeField]
        private float cellAppearingSpeed = 0.5f;
        
        private readonly List<CellVisualizer> _visualizedCells = new List<CellVisualizer>();
        private readonly List<CellVisualizer> _fadeOutCells = new List<CellVisualizer>();
        
        private readonly List<CellVisualizer> _pool = new List<CellVisualizer>();

        private List<CellVisualizer> _usedVisualizers = new List<CellVisualizer>();
        private List<CellVisualizer> _newVisualizers = new List<CellVisualizer>();
        
        public void Move()
        {
            var delta = cellAppearingSpeed * Time.deltaTime;
            
            foreach (var visualizer in _visualizedCells)
            {
                visualizer.Move(delta);
            }

            for (var i = 0; i < _fadeOutCells.Count; )
            {
                _fadeOutCells[i].Move(delta);

                if (_fadeOutCells[i].State != CellVisualizer.VisualizerState.Stay)
                {
                    ++i;
                    
                    continue;
                }

                _fadeOutCells[i].gameObject.SetActive(false);
                _fadeOutCells.Remove(_fadeOutCells[i]);
            }
        }
        
        public void VisualizeCells(IReadOnlyCollection<LocationCell> cells, Transform pointedGrid)
        {
            GetVisualizers(cells, pointedGrid);

            SetUnusedCellsVisualizersToFadeout();

            _visualizedCells.AddRange(_newVisualizers);
        }

        private void SetUnusedCellsVisualizersToFadeout()
        {
            for (var i = 0; i < _visualizedCells.Count;)
            {
                if (_usedVisualizers.Contains(_visualizedCells[i]))
                {
                    ++i;

                    continue;
                }

                _visualizedCells[i].SetState(CellVisualizer.VisualizerState.FadeOut);

                _fadeOutCells.Add(_visualizedCells[i]);
                _visualizedCells.Remove(_visualizedCells[i]);
            }
        }

        private void GetVisualizers(IReadOnlyCollection<LocationCell> cells, Transform pointedGrid)
        {
            _usedVisualizers.Clear();
            _newVisualizers.Clear();
            
            foreach (var cell in cells)
            {
                if (TryGetExistedCellVisualizer(cell, out var oldVisualizer))
                {
                    _usedVisualizers.Add(oldVisualizer);

                    continue;
                }

                var cellVisualizer = GetAndSetNewCellVisualizer(pointedGrid, cell);

                _newVisualizers.Add(cellVisualizer);
            }
        }

        private CellVisualizer GetAndSetNewCellVisualizer(Transform pointedGrid, LocationCell cell)
        {
            var cellVisualizer = GetCellBuildingVisualizer();

            var localPosition = cell.GetPosition();

            var position = pointedGrid.TransformPoint(localPosition);

            cellVisualizer.transform.SetPositionAndRotation(position, pointedGrid.rotation);

            cellVisualizer.Init(cell.Index, cell.IsOccupied);

            cellVisualizer.gameObject.SetActive(true);

            return cellVisualizer;
        }

        public void FadeOutCurrentCells()
        {
            _fadeOutCells.AddRange(_visualizedCells);
            _visualizedCells.Clear();
        }
        
        private CellVisualizer GetCellBuildingVisualizer()
        {
            foreach (var visualizer in _pool)
            {
                if (visualizer.gameObject.activeSelf == false)
                {
                    return visualizer;
                }
            }
            
            var instanced = Object.Instantiate
            (
                cellPrefab,
                container
            );

            _pool.Add(instanced);
            
            return instanced;
        }

        private bool TryGetExistedCellVisualizer(LocationCell cell, out CellVisualizer visualizer)
        {
            foreach (var oldVisualizer in _visualizedCells)
            {
                if (oldVisualizer.Index == cell.Index)
                {
                    visualizer = oldVisualizer;

                    return true;
                }
            }

            visualizer = null;
            
            return false;
        }
    }
}
