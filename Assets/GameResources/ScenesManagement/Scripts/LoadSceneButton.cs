using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameResources.ScenesManagement.Scripts
{
    public sealed class LoadSceneButton : MonoBehaviour
    {
        [SerializeField]
        private SceneReference scene;

        [SerializeField]
        private Button button;

        private SceneLoaderEventHandler _eventHandler;

        [Inject]
        private void Construct(SceneLoaderEventHandler sceneLoaderEventHandler)
        {
            _eventHandler = sceneLoaderEventHandler;
        }

        private void OnEnable() => button.onClick.AddListener(Load);

        private void OnDisable() => button.onClick.RemoveListener(Load);

        private void Load() => _eventHandler.RequestLoad(scene);
    }
}