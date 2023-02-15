using System;
using System.Collections.Generic;
using GameResources.Control.Building.Scripts;
using GameResources.Utility.Editor;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace GameResources.Control.Builder.Scripts
{
    [CreateAssetMenu
    (
        fileName = "BuildingsDataCollector",
        menuName = "Builder/BuildingsDataCollector"
    )]
    public sealed class BuildingsDataCollector : ScriptableObjectInstaller
    {
        public event Action OnInited;
        
        public bool IsInited { get; private set; }

        [SerializeField]
        private List<BuildingData> buildings = new List<BuildingData>();
        public IReadOnlyList<BuildingData> Buildings => buildings;

        public override void InstallBindings()
        {
            Container.Bind<BuildingsDataCollector>().FromInstance(this);
            
            if (IsInited)
            {
                return;
            }

            IsInited = true;
            OnInited?.Invoke();
        }

#if UNITY_EDITOR

        public void GetBuildings()
        {
            buildings.Clear();
            
            var assets = AssetsUtils.GetAssets(typeof(BuildingData), nameof(BuildingData));

            foreach (var asset in assets)
            {
                buildings.Add(asset as BuildingData);
            }
            
            EditorUtility.SetDirty(this);
        }
        
#endif
        
    }
}
