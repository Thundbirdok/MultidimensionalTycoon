using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace GameResources.Location.Building.Scripts
{
    [CreateAssetMenu(fileName = "BuildingsViewDataCollector", menuName = "Builder/BuildingsViewDataCollector")]
    public class BuildingsViewDataCollector : ScriptableObjectInstaller
    {
        public event Action OnInited;
        
        public bool IsInited { get; private set; }

        [SerializeField]
        private List<BuildingViewData> buildings;

        public IReadOnlyList<BuildingViewData> Buildings => buildings;

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
            var guids = AssetDatabase.FindAssets("t:"+ nameof(BuildingViewData));

            buildings = new List<BuildingViewData>(guids.Length);
            
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                buildings.Add(AssetDatabase.LoadAssetAtPath<BuildingViewData>(path));
            }
            
            EditorUtility.SetDirty(this);
        }
        
#endif
    }
}
