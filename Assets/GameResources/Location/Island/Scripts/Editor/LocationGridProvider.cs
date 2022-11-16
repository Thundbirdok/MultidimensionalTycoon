using UnityEditor;
using UnityEngine;

namespace GameResources.Location.Island.Scripts.Editor
{
    [CustomEditor(typeof(LocationGridProvider))]
    public sealed class LocationGridProviderInspector : UnityEditor.Editor
    {
        private LocationGridProvider _locationGrid;
        
        private void OnEnable() => _locationGrid = target as LocationGridProvider;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Clear cells duplicates"))
            {
                _locationGrid.ClearCellDuplicates();
            }
            
            if (GUILayout.Button("Sort cells"))
            {
                _locationGrid.SortCells();
            }
        }

        [DrawGizmo(GizmoType.Selected)]
        private static void DrawGridPlane(LocationGridProvider locationGrid, GizmoType gizmoType)
        {
            var gizmoDefaultColor = Gizmos.color;
            Gizmos.color = Color.cyan;
            
            var gizmoDefaultMatrix = Gizmos.matrix;
            Gizmos.matrix = locationGrid.transform.localToWorldMatrix;
            
            var size = new Vector3(locationGrid.Grid.Size.x, 0, locationGrid.Grid.Size.y);
            
            Gizmos.DrawWireCube
            (
                Vector3.zero,
                size
            );
            
            Gizmos.matrix = gizmoDefaultMatrix;            
            
            Gizmos.color = gizmoDefaultColor;
        }
        
        [DrawGizmo(GizmoType.InSelectionHierarchy)]
        private static void DrawCells(LocationGridProvider locationGrid, GizmoType gizmoType)
        {
            if (locationGrid.Grid == null)
            {
                return;
            }
            
            var gizmoDefaultMatrix = Gizmos.matrix; 
            Gizmos.matrix = locationGrid.transform.localToWorldMatrix;
            
            var gizmoDefaultColor = Gizmos.color;
            Gizmos.color = Color.yellow;

            var cellOffset = Vector2.one * locationGrid.Grid.CellSize / 2;
            var offset = locationGrid.Grid.Size / 2 - cellOffset;

            var offsetV3 = new Vector3(offset.x, 0, offset.y);

            var cellSize = new Vector3(locationGrid.Grid.CellSize, 0, locationGrid.Grid.CellSize);
            
            foreach (var cell in locationGrid.Grid.Cells)
            {
                var position = cell.Index * Vector2.one * locationGrid.Grid.CellSize;
                
                var center = new Vector3(position.x, 0, position.y) - offsetV3;
                
                Gizmos.DrawWireCube(center, cellSize);
            }
            
            Gizmos.matrix = gizmoDefaultMatrix;
            Gizmos.color = gizmoDefaultColor;
        }
    }
}