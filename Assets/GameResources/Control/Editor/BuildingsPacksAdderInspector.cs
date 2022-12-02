using GameResources.Control.Builder.Scripts;
using UnityEditor;
using UnityEngine;

namespace GameResources.Control.Editor
{
    [CustomEditor(typeof(BuildingsPacksAdder))]
    public class BuildingsPacksAdderInspector : UnityEditor.Editor
    {
        private BuildingsPacksAdder _adder;
        
        private void OnEnable() => _adder = target as BuildingsPacksAdder;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Add new pack"))
            {
                _adder.AddNewPack();
            }
        }
    }
}
