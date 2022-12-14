using System;
using System.Collections.Generic;
using GameResources.Utility.Editor;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace GameResources.Location.Building.Scripts
{
    [CreateAssetMenu(fileName = "BuildingsVisualDataCollector", menuName = "Builder/BuildingsVisualDataCollector")]
    public sealed class BuildingsVisualDataCollector : ScriptableObjectInstaller
    {
        public event Action OnInited;

        public bool IsInited { get; private set; }

        [SerializeField]
        private List<BuildingVisualData> visuals = new List<BuildingVisualData>();

        public IReadOnlyList<BuildingVisualData> Visuals => visuals;

        public override void InstallBindings()
        {
            Container.Bind<BuildingsVisualDataCollector>().FromInstance(this);

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
            visuals.Clear();

            var assets = AssetsUtils.GetAssets
            (
                typeof(BuildingVisualData), 
                nameof(BuildingVisualData)
            );

            foreach (var asset in assets)
            {
                visuals.Add(asset as BuildingVisualData);
            }

            EditorUtility.SetDirty(this);
        }
    
#endif
    
    }
}
