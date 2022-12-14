using GameResources.Location.Building.Scripts.Visualizer;
using GameResources.Utility.Editor;
using UnityEditor;

namespace GameResources.Location.Editor
{
    [CustomEditor(typeof(BuildingVisualizer))]
    public sealed class BuildingVisualizerInspector : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active)]
        private static void DrawCells(BuildingVisualizer builderVisualizer, GizmoType gizmoType)
        {
            if (builderVisualizer.Data == null)
            {
                return;
            }
            
            CellSizeDrawer.Draw
            (
                builderVisualizer.Data.Size,
                builderVisualizer.transform.localToWorldMatrix
            );
        }
    }
}
