using UnityEditor;
using UnityEngine;

namespace GameResources.Location.Scripts.Editor
{
    [CustomEditor(typeof(GridProvider))]
    public sealed class GridProviderInspector : UnityEditor.Editor
    {
        private GridProvider _grid;
        
        private void OnEnable()
        {
            _grid = target as GridProvider;
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Clear duplicates"))
            {
                _grid.ClearDuplicates();
            }
        }

        [DrawGizmo(GizmoType.Selected)]
        private static void DrawGridPlane(GridProvider grid, GizmoType gizmoType)
        {
            var gizmoDefaultColor = Gizmos.color;
            Gizmos.color = Color.cyan;
            
            Gizmos.DrawWireCube
            (
                grid.transform.position, 
                grid.transform.TransformVector(new Vector3(grid.Grid.Size.x, 0, grid.Grid.Size.y))
            );
            
            Gizmos.color = gizmoDefaultColor;
        }
        
        [DrawGizmo(GizmoType.InSelectionHierarchy)]
        private static void DrawCells(GridProvider grid, GizmoType gizmoType)
        {
            if (grid.Grid == null)
            {
                return;
            }
            
            var gizmoDefaultColor = Gizmos.color;
            Gizmos.color = Color.yellow;

            var cellOffset = Vector2.one * grid.Grid.CellSize / 2;
            var offset = grid.Grid.Size / 2 - cellOffset;

            var offsetV3 = new Vector3(offset.x, 0, offset.y);

            foreach (var cell in grid.Grid.Cells)
            {
                var position = cell.Position * Vector2.one * grid.Grid.CellSize;

                DrawRect
                (
                    grid,
                    new Vector3(position.x, 0, position.y) 
                    - offsetV3
                );
            }
            
            Gizmos.color = gizmoDefaultColor;
        }

        private static void DrawRect(GridProvider grid, Vector3 center)
        {
            var localCenter = grid.transform.TransformPoint(center);
            
            var size = new Vector3(grid.Grid.CellSize, 0, grid.Grid.CellSize);
            var localSize = grid.transform.TransformVector(size);

            Gizmos.DrawWireCube(localCenter, localSize);
        }
    }
}