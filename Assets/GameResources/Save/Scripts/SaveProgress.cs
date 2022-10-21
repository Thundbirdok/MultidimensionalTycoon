using GameResources.ScenesManagement.Scripts;
using UnityEngine;
using Zenject;

namespace GameResources.Save.Scripts
{
    public sealed class SaveProgress : MonoBehaviour
    {
        [SerializeField]
        private SceneContext sceneContext;

        private SceneLoaderEventHandler _eventHandler;
        
        [Inject]
        private void Construct(SceneLoaderEventHandler sceneLoaderEventHandler)
        {
            _eventHandler = sceneLoaderEventHandler;

            _eventHandler.OnBeginUnload += Save;
        }
        
        private void OnDestroy() => _eventHandler.OnBeginUnload -= Save;

        private void Save()
        {
            var list = sceneContext.Container.ResolveAll<ISaveProgress>();
            
            foreach(var save in list)
            {
                save.Save();
            }
        }
    }
}
