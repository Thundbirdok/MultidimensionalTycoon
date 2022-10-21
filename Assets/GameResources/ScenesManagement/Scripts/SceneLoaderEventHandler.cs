using System;
using UnityEditor;
using UnityEngine;

namespace GameResources.ScenesManagement.Scripts
{
    [CreateAssetMenu(fileName = "SceneLoaderEventHandler", menuName = "SceneManagement/SceneLoaderEventHandler")]
    public sealed class SceneLoaderEventHandler : ScriptableObject
    {
        public event Action OnBeginUnload;
        public event Action<SceneAsset> OnLoadRequest;
        
        public void BeginUnloadInvoke() => OnBeginUnload?.Invoke();

        public void RequestLoad(SceneAsset scene) => OnLoadRequest?.Invoke(scene);
    }
}