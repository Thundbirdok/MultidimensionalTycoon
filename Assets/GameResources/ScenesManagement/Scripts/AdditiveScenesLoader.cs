using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GameResources.ScenesManagement.Scripts
{
    public sealed class AdditiveScenesLoader : MonoBehaviour
    {
        [SerializeField] 
        private string folder = "/Scenes/.../";

        [SerializeField] 
        private List<SceneAsset> scenesAssets;

        private readonly List<Scene> _openedScenes = new();

        public async Task Load()
        {
            List<Task<SceneInstance>> tasks = new();

            foreach (var sceneAsset in scenesAssets)
            {
                var task = LoadAdditiveScene(sceneAsset.name);

                tasks.Add(task);
            }

            await Task.WhenAll(tasks.ToArray());

            _openedScenes.AddRange
            (
                tasks.Select
                (
                    x => x.Result.Scene
                )
            );
        }

        public void Unload()
        {
            foreach (var scene in _openedScenes) SceneManager.UnloadSceneAsync(scene);
        }

        private Task<SceneInstance> LoadAdditiveScene(string sceneName)
        {
            return Addressables.LoadSceneAsync
            (
                sceneName,
                LoadSceneMode.Additive
            ).Task;
        }

#if UNITY_EDITOR

        public void OpenAdditiveScenesInEditor()
        {
            foreach (var sceneAsset in scenesAssets)
            {
                var scene = OpenScene(sceneAsset.name);

                _openedScenes.Add(scene);
            }
        }

        private Scene OpenScene(string sceneName)
        {
            var scenePath =
                Application.dataPath
                + folder
                + sceneName
                + ".unity";

            return EditorSceneManager.OpenScene
            (
                scenePath,
                OpenSceneMode.Additive
            );
        }

        public void CloseAdditiveScenesInEditor()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.SaveScenes(_openedScenes.ToArray());

            foreach (var scene in _openedScenes)
                EditorSceneManager.CloseScene
                (
                    scene,
                    true
                );
        }

#endif
    }
}