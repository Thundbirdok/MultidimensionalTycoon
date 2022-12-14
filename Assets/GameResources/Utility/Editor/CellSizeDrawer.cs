using UnityEngine;

namespace GameResources.Utility.Editor
{
    public sealed class CellSizeDrawer : MonoBehaviour
    {
        public static void Draw(int size, Matrix4x4 localToWorldMatrix)
        {
            if (size <= 0)
            {
                return;
            }
            
            var gizmoDefaultMatrix = Gizmos.matrix; 
            Gizmos.matrix = localToWorldMatrix;
            
            var gizmoDefaultColor = Gizmos.color;
            Gizmos.color = Color.magenta;
            
            var axisGridOffset = 0f;

            int indexOffset;

            if (size % 2 == 0)
            {
                indexOffset = (size / 2) - 1;
                axisGridOffset = 0.5f;
            }
            else
            {
                indexOffset = (size - 1) / 2;
            }
            
            var gridOffset = new Vector3(axisGridOffset, 0, axisGridOffset);
            
            var leftDown = new Vector2Int(indexOffset, indexOffset);

            var cellSize = new Vector3(1, 0, 1);
            
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
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
