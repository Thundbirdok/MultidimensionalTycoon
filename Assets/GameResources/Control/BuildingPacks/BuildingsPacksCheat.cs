namespace GameResources.Control.BuildingPacks
{
    using GameResources.Control.Builder.Scripts;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public sealed class BuildingsPacksCheat : MonoBehaviour
    {
        [SerializeField]
        private Button add;
        
        private BuilderEventHandler _builderEventHandler;

        [Inject]
        private void Construct(BuilderEventHandler builderEventHandler)
        {
            _builderEventHandler = builderEventHandler;
            
            add.onClick.AddListener(Add);
        }

        private void OnDestroy()
        {
            add.onClick.RemoveListener(Add);
        }
        
        private void Add() => _builderEventHandler.InvokeRequestAddPack();
    }
}
