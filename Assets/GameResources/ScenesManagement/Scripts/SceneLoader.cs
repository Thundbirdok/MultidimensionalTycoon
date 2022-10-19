using GameResources.Effects.SceneFadeInOut.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameResources.ScenesManagement.Scripts
{
    public class SceneLoader : MonoBehaviour
    {
        private SceneLoaderEventHandler _eventHandler;
        private SceneFadeInOutProvider _sceneFadeInOutProvider;
        private AdditiveScenesLoader _additiveScenesLoader;

        [Inject]
        private void Construct
        (
            SceneLoaderEventHandler sceneLoaderEventHandler,
            SceneFadeInOutProvider fadeInOutProvider,
            AdditiveScenesLoader additiveScenesLoader
        )
        {
            _eventHandler = sceneLoaderEventHandler;
            _sceneFadeInOutProvider = fadeInOutProvider;
            _additiveScenesLoader = additiveScenesLoader;

            Subscribe();
        }

        private async void Start()
        {
            await _additiveScenesLoader.Load();

            await _sceneFadeInOutProvider.FadeIn();
        }

        private void OnDestroy() => Unsubscribe();

        private void Subscribe()
        {
            _eventHandler.OnLoadRequest += Load;
        }

        private void Unsubscribe()
        {
            if (_eventHandler == null)
            {
                return;
            }

            _eventHandler.OnLoadRequest -= Load;
        }

        private async void Load(SceneAsset targetScene)
        {
            await _sceneFadeInOutProvider.FadeOut();

            _eventHandler.BeginUnloadInvoke();
            
            _additiveScenesLoader.Unload();

            SceneManager.LoadSceneAsync(targetScene.name);
        }
    }
}
