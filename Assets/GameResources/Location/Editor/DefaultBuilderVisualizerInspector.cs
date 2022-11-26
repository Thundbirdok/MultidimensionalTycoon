using GameResources.Location.Building.Scripts.BuildingVisualizer.Default;
using UnityEditor;
using UnityEngine;

namespace GameResources.Location.Editor
{
    [CustomEditor(typeof(DefaultBuildingVisualizer))]
    public class DefaultBuilderVisualizerInspector : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active)]
        private static void DrawCells(DefaultBuildingVisualizer builderVisualizer, GizmoType gizmoType)
        {
            var buildingData = builderVisualizer.BuildingData;

            if (buildingData == null)
            {
                return;
            }
            
            var gizmoDefaultMatrix = Gizmos.matrix; 
            Gizmos.matrix = builderVisualizer.transform.localToWorldMatrix;
            
            var gizmoDefaultColor = Gizmos.color;
            Gizmos.color = Color.magenta;
            
            var axisGridOffset = 0f;

            int indexOffset;

            if (buildingData.Size % 2 == 0)
            {
                indexOffset = (buildingData.Size / 2) - 1;
                axisGridOffset = 0.5f;
            }
            else
            {
                indexOffset = (buildingData.Size - 1) / 2;
            }
            
            var gridOffset = new Vector3(axisGridOffset, 0, axisGridOffset);
            
            var leftDown = new Vector2Int(indexOffset, indexOffset);

            var cellSize = new Vector3(1, 0, 1);
            
            for (int i = 0; i < buildingData.Size; ++i)
            {
                for (int j = 0; j < buildingData.Size; ++j)
                {
                    var cellIndex = -leftDown + new Vector2Int(j, i);

                    var position = cellIndex * Vector2.one;
                    
                    var center = new Vector3(position.x, 0, position.y) - gridOffset;

                    Gizmos.DrawWireCube(center, cellSize);
                }
            }
            
            Gizmos.matrix = gizmoDefaultMatrix;
            Gizmos.color = gizmoDefaultColor;
        }
    }
}
