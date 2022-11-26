using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

#if  UNITY_EDITOR

using UnityEditor.SceneManagement;

#endif

namespace GameResources.ScenesManagement.Scripts
{
    public sealed class AdditiveScenesLoader : MonoBehaviour
    {
        [SerializeField]
        private List<SceneReference> scenesAssets;
        
        private readonly List<Scene> _openedScenes = new();

        public async Task Load()
        {
            var tasks = new List<Task<Scene>>();

            foreach (var sceneAsset in scenesAssets)
            {
                var task = LoadAdditiveScene(sceneAsset.ScenePath);

                tasks.Add(task);
            }

            await Task.WhenAll(tasks.ToArray());

            _openedScenes.AddRange
            (
                tasks.Select
                (
                    x => x.Result
                )
            );
        }

        public void Unload()
        {
            foreach (var scene in _openedScenes)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        private async static Task<Scene> LoadAdditiveScene(string sceneName)
        {
            var asyncOperation = SceneManager.LoadSceneAsync
            (
                sceneName.Split('/')[^1].Split('.')[0],
                LoadSceneMode.Additive
            );

            asyncOperation.allowSceneActivation = true;
            
            while (asyncOperation.progress < 1)
            {
                await Task.Yield();
            }

            var scene = SceneManager.GetSceneByName(sceneName.Split('/')[^1].Split('.')[0]);
            
            return scene;
        }

#if UNITY_EDITOR

        public void OpenAdditiveScenesInEditor()
        {
            foreach (var sceneAsset in scenesAssets)
            {
                var scene = OpenScene(sceneAsset.ScenePath);

                _openedScenes.Add(scene);
            }
        }

        private static Scene OpenScene(string sceneName)
        {
            return EditorSceneManager.OpenScene
            (
                sceneName,
                OpenSceneMode.Additive
            );
        }

        public void CloseAdditiveScenesInEditor()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.SaveScenes(_openedScenes.ToArray());
            }

            foreach (var scene in _openedScenes)
            {
                EditorSceneManager.CloseScene
                (
                    scene,
                    true
                );
            }
        }

#endif
    }
}