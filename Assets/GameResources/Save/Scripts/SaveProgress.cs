using UnityEngine;
using Zenject;

namespace GameResources.Save.Scripts
{
    public sealed class SaveProgress : MonoBehaviour
    {
        [SerializeField]
        private SceneContext sceneContext;
        
        private void OnDestroy() => Save();

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
