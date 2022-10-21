using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameResources.ScenesManagement.Scripts
{
    public sealed class LoadSceneOnStart : MonoBehaviour
    {
        [SerializeField]
        private SceneAsset scene;

        private void Start() => SceneManager.LoadSceneAsync(scene.name);
    }
}
