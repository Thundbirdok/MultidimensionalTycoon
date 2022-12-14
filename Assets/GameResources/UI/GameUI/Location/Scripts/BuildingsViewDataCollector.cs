using System;
using System.Collections.Generic;
using GameResources.Utility.Editor;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace GameResources.UI.GameUI.Location.Scripts
{
    [CreateAssetMenu(fileName = "BuildingsViewDataCollector", menuName = "Builder/BuildingsViewDataCollector")]
    public sealed class BuildingsViewDataCollector : ScriptableObjectInstaller
    {
        public event Action OnInited;

        public bool IsInited { get; private set; }

        [SerializeField]
        private List<BuildingViewData> views = new List<BuildingViewData>();

        public IReadOnlyList<BuildingViewData> Views => views;

        public override void InstallBindings()
        {
            Container.Bind<BuildingsViewDataCollector>().FromInstance(this);

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
            views.Clear();

            var assets = AssetsUtils.GetAssets(typeof(BuildingViewData), nameof(BuildingViewData));

            foreach (var asset in assets)
            {
                views.Add(asset as BuildingViewData);
            }

            EditorUtility.SetDirty(this);
        }
        
#endif
        
    }
}