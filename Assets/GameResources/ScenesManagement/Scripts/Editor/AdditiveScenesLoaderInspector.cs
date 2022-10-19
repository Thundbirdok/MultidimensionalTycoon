using UnityEditor;
using UnityEngine;

namespace GameResources.ScenesManagement.Scripts.Editor
{
    [CustomEditor(typeof(AdditiveScenesLoader))]
    public class AdditiveScenesLoaderInspector : UnityEditor.Editor
    {
        private AdditiveScenesLoader _sceneStartup;

        private void OnEnable()
        {
            _sceneStartup = target as AdditiveScenesLoader;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Open additive scenes"))
            {
                _sceneStartup.OpenAdditiveScenesInEditor();
            }

            if (GUILayout.Button("Close additive scenes"))
            {
                _sceneStartup.CloseAdditiveScenesInEditor();
            }
        }
    }
}