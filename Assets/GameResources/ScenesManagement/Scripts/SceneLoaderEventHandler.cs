using System;
using UnityEngine;

namespace GameResources.ScenesManagement.Scripts
{
    [CreateAssetMenu(fileName = "SceneLoaderEventHandler", menuName = "SceneManagement/SceneLoaderEventHandler")]
    public sealed class SceneLoaderEventHandler : ScriptableObject
    {
        public event Action<SceneReference> OnLoadRequest;

        public void RequestLoad(SceneReference scene) => OnLoadRequest?.Invoke(scene);
    }
}