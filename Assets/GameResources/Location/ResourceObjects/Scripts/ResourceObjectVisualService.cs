using System.Collections.Generic;
using System.Threading.Tasks;
using GameResources.Control.ResourceObjects.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Location.ResourceObjects.Scripts
{
    using System;
    using Random = UnityEngine.Random;

    public sealed class ResourceObjectVisualService : MonoBehaviour
    {
        [field: SerializeField]
        public List<AssetReference> Models { get; private set; }

        [NonSerialized]
        private bool _isModelLoaded;
        
        [NonSerialized]
        private int _modelIndex = -1;
        
        [NonSerialized]
        private GameObject _model;
        
        #if UNITY_EDITOR

        [field:SerializeField]
        public ResourceObjectData Data { get; private set; }

        #endif

        private void OnEnable() => _ = InstantiateModel();

        private void OnDisable() => ReleaseModel();

        public async void SetModel(int modelIndex)
        {
            var newIndex = modelIndex % Models.Count;

            if (_modelIndex == newIndex)
            {
                return;
            }

            ReleaseModel();
                
            _modelIndex = newIndex;
                
            await InstantiateModel();
        }
        
        private async Task InstantiateModel()
        {
            if (_modelIndex == -1)
            {
                return;
            }
            
            if (_isModelLoaded)
            {
                return;
            }
            
            _isModelLoaded = true;
            
            _model = await Models[_modelIndex]
                .InstantiateAsync
                (
                    transform
                )
                .Task;

            _model.transform.localRotation = Quaternion.Euler(0, Random.Range(-180, 180), 0);
        }

        public void ReleaseModel()
        {
            if (_isModelLoaded == false)
            {
                return;
            }

            if (_model == null)
            {
                return;
            }
            
            _isModelLoaded = false;
            
            Models[_modelIndex].ReleaseInstance(_model);
            _model = null;
        }
    }
}
