namespace GameResources.Control.ResourceObjects.Scripts
{
    using System;
    using System.Collections.Generic;
    using GameResources.Control.Building.Scripts;
    using GameResources.Utility.Editor;
    using NaughtyAttributes;
    using UnityEditor;
    using UnityEngine;
    using Zenject;

    [CreateAssetMenu
    (
        fileName = "ResourceObjectsDataCollector",
        menuName = "ResourceObjects/ResourceObjectsDataCollector"
    )]
    public sealed class ResourceObjectsDataCollector : ScriptableObjectInstaller
    {
        public event Action OnInited;
        
        public bool IsInited { get; private set; }

        [SerializeField]
        private List<ResourceObjectData> resourceObjects = new List<ResourceObjectData>();
        public IReadOnlyList<ResourceObjectData> ResourceObjects => resourceObjects;

        public override void InstallBindings()
        {
            Container.Bind<ResourceObjectsDataCollector>().FromInstance(this);
            
            if (IsInited)
            {
                return;
            }

            IsInited = true;
            OnInited?.Invoke();
        }

#if UNITY_EDITOR

        [Button]
        public void GetBuildings()
        {
            resourceObjects.Clear();
            
            var assets = AssetsUtils.GetAssets(typeof(ResourceObjectData), nameof(ResourceObjectData));

            foreach (var asset in assets)
            {
                if (asset is BuildingData)
                {
                    continue;
                }
                
                resourceObjects.Add(asset as ResourceObjectData);
            }
            
            EditorUtility.SetDirty(this);
        }
        
#endif
        
    }
}
