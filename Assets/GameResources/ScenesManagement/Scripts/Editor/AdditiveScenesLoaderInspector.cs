using UnityEditor;
using UnityEngine;

namespace GameResources.ScenesManagement.Scripts.Editor
{
    [CustomEditor(typeof(AdditiveScenesLoader))]
    public sealed class AdditiveScenesLoaderInspector : UnityEditor.Editor
    {
        private AdditiveScenesLoader _additiveScenesLoader;

        private void OnEnable()
        {
            _additiveScenesLoader = target as AdditiveScenesLoader;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Open additive scenes"))
            {
                _additiveScenesLoader.OpenAdditiveScenesInEditor();
            }

            if (GUILayout.Button("Close additive scenes"))
            {
                _additiveScenesLoader.CloseAdditiveScenesInEditor();
            }
        }
    }
}