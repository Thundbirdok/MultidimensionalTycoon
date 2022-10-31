using System;
using System.Collections.Generic;
using GameResources.Location.Building.Scripts;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace GameResources.Location.Builder.Scripts
{
    [CreateAssetMenu(fileName = "BuildingsViewDataCollector", menuName = "Builder/BuildingsViewDataCollector")]
    public class BuildingsViewDataCollector : ScriptableObjectInstaller
    {
        public event Action OnInited;
        
        public bool IsInited { get; private set; }
        
        public List<BuildingViewData> Buildings { get; private set; }

        public override void InstallBindings()
        {
            Container.Bind<BuildingsViewDataCollector>().FromInstance(this);
            
            if (IsInited)
            {
                return;
            }
            
            GetBuildings();

            IsInited = true;
            OnInited?.Invoke();
        }
        
        private void GetBuildings()
        {
            var guids = AssetDatabase.FindAssets("t:"+ nameof(BuildingViewData));

            Buildings = new List<BuildingViewData>(guids.Length);
            
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                Buildings.Add(AssetDatabase.LoadAssetAtPath<BuildingViewData>(path));
            }
        }
    }
}
