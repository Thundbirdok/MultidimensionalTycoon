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
            
            var size = new Vector3(locationGrid.LocationGrid.Size.x, 0, locationGrid.LocationGrid.Size.y);
            
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
            if (locationGrid.LocationGrid == null)
            {
                return;
            }
            
            var gizmoDefaultColor = Gizmos.color;
            Gizmos.color = Color.yellow;

            var cellOffset = Vector2.one * locationGrid.LocationGrid.CellSize / 2;
            var offset = locationGrid.LocationGrid.Size / 2 - cellOffset;

            var offsetV3 = new Vector3(offset.x, 0, offset.y);

            foreach (var cell in locationGrid.LocationGrid.Cells)
            {
                var position = cell.Index * Vector2.one * locationGrid.LocationGrid.CellSize;

                DrawRect
                (
                    locationGrid,
                    new Vector3(position.x, 0, position.y) 
                    - offsetV3
                );
            }
            
            Gizmos.color = gizmoDefaultColor;
        }

        private static void DrawRect(LocationGridProvider locationGrid, Vector3 center)
        {            
            var size = new Vector3(locationGrid.LocationGrid.CellSize, 0, locationGrid.LocationGrid.CellSize);
                        
            var gizmoDefaultMatrix = Gizmos.matrix; 
            Gizmos.matrix = locationGrid.transform.localToWorldMatrix;

            Gizmos.DrawWireCube(center, size);
            
            Gizmos.matrix = gizmoDefaultMatrix;
        }
    }
}