using GameResources.Control.Builder.Scripts;
using GameResources.Control.Scripts;
using UnityEditor;
using UnityEngine;

namespace GameResources.Control.Editor
{
    [CustomEditor(typeof(BuildingsDataCollector))]
    public class BuildingsDataCollectorInspector : UnityEditor.Editor
    {
        private BuildingsDataCollector _collector;
        
        private void OnEnable() => _collector = target as BuildingsDataCollector;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Get buildings"))
            {
                _collector.GetBuildings();
            }
        }
    }
}
