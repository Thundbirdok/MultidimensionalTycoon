using System;
using System.Collections.Generic;
using GameResources.Utility.Editor;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace GameResources.Location.ResourceObjects.Scripts
{
    [CreateAssetMenu
    (
        fileName = "ResourceObjectVisualDataCollector",
        menuName = "ResourceObjects/ResourceObjectVisualDataCollector"
    )]
    public sealed class ResourceObjectVisualDataCollector : ScriptableObjectInstaller
    {
        public event Action OnInited;

        public bool IsInited { get; private set; }

        [SerializeField]
        private List<ResourceObjectVisualData> visuals = new List<ResourceObjectVisualData>();
        public IReadOnlyList<ResourceObjectVisualData> Visuals => visuals;

        public override void InstallBindings()
        {
            Container.Bind<ResourceObjectVisualDataCollector>().FromInstance(this);

            if (IsInited)
            {
                return;
            }

            IsInited = true;
            OnInited?.Invoke();
        }

#if UNITY_EDITOR

        [Button]
        public void GetObjects()
        {
            visuals.Clear();

            var assets = AssetsUtils.GetAssets
            (
                typeof(ResourceObjectVisualData), 
                nameof(ResourceObjectVisualData)
            );

            foreach (var asset in assets)
            {
                visuals.Add(asset as ResourceObjectVisualData);
            }

            EditorUtility.SetDirty(this);
        }
    
#endif
    }
}
