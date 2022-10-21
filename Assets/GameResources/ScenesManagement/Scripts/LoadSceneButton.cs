using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameResources.ScenesManagement.Scripts
{
    public sealed class LoadSceneButton : MonoBehaviour
    {
        [SerializeField]
        private SceneAsset scene;

        [SerializeField]
        private Button button;

        private SceneLoaderEventHandler eventHandler;

        [Inject]
        private void Construct(SceneLoaderEventHandler sceneLoaderEventHandler)
        {
            eventHandler = sceneLoaderEventHandler;
        }

        private void OnEnable() => button.onClick.AddListener(Load);

        private void OnDisable() => button.onClick.RemoveListener(Load);

        private void Load() => eventHandler.RequestLoad(scene);
    }
}