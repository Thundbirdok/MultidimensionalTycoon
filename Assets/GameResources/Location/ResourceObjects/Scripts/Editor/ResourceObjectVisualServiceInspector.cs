using GameResources.Utility.Editor;
using UnityEditor;

namespace GameResources.Location.ResourceObjects.Scripts.Editor
{
    using UnityEngine;

    [CustomEditor(typeof(ResourceObjectVisualService))]
    public class ResourceObjectVisualServiceInspector : UnityEditor.Editor
    {
        private ResourceObjectVisualService _resourceObjectVisualService;

        private int _index;
        
        private void OnEnable()
        {
            _resourceObjectVisualService = target as ResourceObjectVisualService;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _index = EditorGUILayout.IntField(_index);
            
            if (GUILayout.Button("Instantiate Model"))
            {
                _resourceObjectVisualService.SetModel(_index);
            }
            
            if (GUILayout.Button("Release Model"))
            {
                _resourceObjectVisualService.ReleaseModel();
            }
        }
        
        [DrawGizmo(GizmoType.Active)]
        private static void DrawCells(ResourceObjectVisualService builderVisualService, GizmoType gizmoType)
        {
            if (builderVisualService.Data == null)
            {
                return;
            }
            
            CellSizeDrawer.Draw
            (
                builderVisualService.Data.Size,
                builderVisualService.transform.localToWorldMatrix
            );
        }
    }
}
