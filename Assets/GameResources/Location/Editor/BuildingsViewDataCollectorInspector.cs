using GameResources.Location.Building.Scripts;
using UnityEditor;
using UnityEngine;

namespace GameResources.Location.Editor
{
    [CustomEditor(typeof(BuildingsViewDataCollector))]
    public class BuildingsViewDataCollectorInspector : UnityEditor.Editor
    {
        private BuildingsViewDataCollector _collector;
        
        private void OnEnable() => _collector = target as BuildingsViewDataCollector;
        
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
