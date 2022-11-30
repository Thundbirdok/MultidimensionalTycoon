using GameResources.Control.Builder.Scripts;
using GameResources.UI.GameUI.Location.Scripts;
using UnityEditor;
using UnityEngine;

namespace GameResources.UI.GameUI.Location.Editor
{
    [CustomEditor(typeof(BuildingsViewDataCollector))]
    public class BuildingViewDataCollectorInspector : UnityEditor.Editor
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
